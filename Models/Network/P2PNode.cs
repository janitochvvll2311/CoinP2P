using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace CoinP2P.Models.Network;

public class P2PNode
{

    public List<string> Log { get; } = new();
    public List<P2PHost> Hosts { get; } = new();
    public List<ulong> Nonces { get; } = new();

    public async Task Receive<T>(P2PContext<T> context) where T : P2PMessage
    {
        if (context.IsOpen)
        {
            try
            {
                var result = await context.Host.Socket!.ReceiveAsync(context.Buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var json = Encoding.UTF8.GetString(context.Buffer.Array!, context.Buffer.Offset, result.Count);
                    var message = JsonSerializer.Deserialize<T>(json);
                    context.Message = message;
                }
                else
                {
                    await context.Host.Socket!.CloseOutputAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Log.Add($"{context.Host.Remote}:{ex.Message}");
            }
        }
    }

    public async Task Spread<T>(P2PContext<T> context) where T : P2PMessage
    {
        if (context.Message != null)
        {
            var json = JsonSerializer.Serialize<T>(context.Message);
            var count = Encoding.UTF8.GetBytes(json, context.Buffer);
            var data = new ArraySegment<byte>(context.Buffer.Array!, context.Buffer.Offset, count);
            var remotes = Hosts.Where(x => x != context.Host).ToArray();
            foreach (var remote in remotes)
            {
                if (remote.IsOpen)
                {
                    try
                    {
                        await remote.Socket!.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Log.Add($"{remote.Remote}:{ex.Message}");
                    }
                }
            }
        }
    }

    public async Task Poll<T>(P2PHost host, Func<T, bool> callback) where T : P2PMessage
    {
        Hosts.Add(host);
        var buffer = new byte[1024];
        var context = new P2PContext<T>(host, buffer);
        while (context.IsOpen)
        {
            try
            {
                await Receive<T>(context);
                if (context.Message != null && !Nonces.Contains(context.Message.Nonce) && callback(context.Message))
                {
                    Nonces.Add(context.Message.Nonce);
                    await Spread<T>(context);
                }
            }
            catch (Exception ex)
            {
                Log.Add($"{host.Remote}:{ex.Message}");
            }
        }
        Hosts.Remove(host);
    }

}