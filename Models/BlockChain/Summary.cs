namespace CoinP2P.Models.BlockChain;

public partial class Summary
{
    public long Id { get; set; }
    public byte[] Source { get; set; } = null!;
    public byte[] Target { get; set; } = null!;
    public long Balance { get; set; }
    public byte[] Hash { get; set; } = null!;
    public byte[] Signature { get; set; } = null!;
    public long BlockId { get; set; }

    public virtual Block Block { get; set; } = null!;
    public virtual KeyPair SourceNavigation { get; set; } = null!;
    public virtual KeyPair TargetNavigation { get; set; } = null!;
}
