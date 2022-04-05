using CoinP2P.Models.Network;

namespace CoinP2P.Models;

public class ChatMessage : P2PMessage
{
    public string? Remote { get; set; }
    public string? Message { get; set; }
    public string? Signature { get; set; }
}