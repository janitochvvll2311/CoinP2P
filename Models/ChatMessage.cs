using CoinP2P.Models.Network;

namespace CoinP2P.Models;

public class ChatMessage : P2PMessage
{
    public string? Message { get; set; }
}