namespace CoinP2P.Models.BlockChain;

public partial class Block
{
    public Block()
    {
        InverseChainHashNavigation = new HashSet<Block>();
        Summaries = new HashSet<Summary>();
        Transactions = new HashSet<Transaction>();
    }

    public long Id { get; set; }
    public byte[] Source { get; set; } = null!;
    public byte[] ChainHash { get; set; } = null!;
    public byte[] Hash { get; set; } = null!;
    public byte[] Signature { get; set; } = null!;

    public virtual Block ChainHashNavigation { get; set; } = null!;
    public virtual KeyPair SourceNavigation { get; set; } = null!;
    public virtual ICollection<Block> InverseChainHashNavigation { get; set; }
    public virtual ICollection<Summary> Summaries { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }
}
