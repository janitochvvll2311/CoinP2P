namespace CoinP2P.Models.BlockChain;

public partial class Transaction
{
    public long Id { get; set; }
    public byte[] Source { get; set; } = null!;
    public byte[] Destination { get; set; } = null!;
    public long Amount { get; set; }
    public byte[] Hash { get; set; } = null!;
    public byte[] Signature { get; set; } = null!;
    public long BlockId { get; set; }

    public virtual Block Block { get; set; } = null!;
    public virtual KeyPair DestinationNavigation { get; set; } = null!;
    public virtual KeyPair SourceNavigation { get; set; } = null!;
}
