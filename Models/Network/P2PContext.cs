using System.Net.WebSockets;

namespace CoinP2P.Models.Network;

public class P2PContext<T> where T : P2PMessage
{

    public P2PHost Host { get; }
    public T? Message { get; set; }
    public ArraySegment<byte> Buffer { get; }
    public bool IsOpen => Host?.Socket?.State == WebSocketState.Open;

    public P2PContext(P2PHost host, ArraySegment<byte> buffer)
    {
        Host = host;
        Buffer = buffer;
    }

}