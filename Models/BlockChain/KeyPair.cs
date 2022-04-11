namespace CoinP2P.Models.BlockChain;

public partial class KeyPair
{
    public KeyPair()
    {
        Blocks = new HashSet<Block>();
        SummarySourceNavigations = new HashSet<Summary>();
        SummaryTargetNavigations = new HashSet<Summary>();
        TransactionDestinationNavigations = new HashSet<Transaction>();
        TransactionSourceNavigations = new HashSet<Transaction>();
    }

    public long Id { get; set; }
    public byte[] Public { get; set; } = null!;
    public byte[]? Private { get; set; }
    public byte[] Hash { get; set; } = null!;

    public virtual ICollection<Block> Blocks { get; set; }
    public virtual ICollection<Summary> SummarySourceNavigations { get; set; }
    public virtual ICollection<Summary> SummaryTargetNavigations { get; set; }
    public virtual ICollection<Transaction> TransactionDestinationNavigations { get; set; }
    public virtual ICollection<Transaction> TransactionSourceNavigations { get; set; }
}
