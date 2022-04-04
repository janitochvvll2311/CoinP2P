using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace CoinP2P.Models.Network;

public class Node
{

    public List<string> Log { get; } = new();
    public List<Host> Hosts { get; } = new();
    public List<ulong> Nonces { get; } = new();

    public async Task<T?> Receive<T>(Host host, ArraySegment<byte> buffer) where T : Message
    {
        if (host.IsOpen)
        {
            try
            {
                var result = await host.Socket!.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var json = Encoding.UTF8.GetString(buffer.Array!, buffer.Offset, buffer.Count);
                    var message = JsonSerializer.Deserialize<T>(json);
                    return message;
                }
                else
                {
                    await host.Socket!.CloseOutputAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                Log.Add($"{host.Remote}:{ex.Message}");
            }
        }
        return null;
    }

    public async Task Spread<T>(T? message, ArraySegment<byte> buffer) where T : Message
    {
        if (message != null)
        {
            var json = JsonSerializer.Serialize<T>(message);
            var count = Encoding.UTF8.GetBytes(json, buffer);
            buffer = new(buffer.Array!, buffer.Offset, buffer.Count);
            foreach (var host in Hosts.ToArray())
            {
                if (host.IsOpen)
                {
                    try
                    {
                        await host.Socket!.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Log.Add($"{host.Remote}:{ex.Message}");
                    }
                }
            }
        }
    }

    public async Task Poll<T>(Host host) where T : Message
    {
        Hosts.Add(host);
        var buffer = new byte[1024];
        while (host.IsOpen)
        {
            try
            {
                var message = await Receive<T>(host, buffer);
                if (message != null && !Nonces.Contains(message.Nonce))
                    await Spread<T>(message, buffer);
            }
            catch (Exception ex)
            {
                Log.Add($"{host.Remote}:{ex.Message}");
            }
        }
        Hosts.Remove(host);
    }

}