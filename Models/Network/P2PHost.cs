using System.Net.WebSockets;

namespace CoinP2P.Models.Network;

public class P2PHost
{
    public string? Remote { get; set; }
    public WebSocket? Socket { get; set; }
    public bool IsOpen => Socket?.State == WebSocketState.Open;
}