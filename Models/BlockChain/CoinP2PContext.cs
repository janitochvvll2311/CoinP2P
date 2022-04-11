using Microsoft.EntityFrameworkCore;

namespace CoinP2P.Models.BlockChain;

public partial class CoinP2PContext : DbContext
{
    public CoinP2PContext()
    {
    }

    public CoinP2PContext(DbContextOptions<CoinP2PContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Block> Blocks { get; set; } = null!;
    public virtual DbSet<KeyPair> KeyPairs { get; set; } = null!;
    public virtual DbSet<Summary> Summaries { get; set; } = null!;
    public virtual DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlite("DataSource=D:/Source/CoinP2P/wwwroot/Data/coinp2p.db3");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Block>(entity =>
        {
            entity.ToTable("Block");

            entity.HasIndex(e => e.Hash, "IX_Block_Hash")
                .IsUnique();

            entity.Property(e => e.ChainHash).HasColumnType("BLOB (32)");

            entity.Property(e => e.Hash).HasColumnType("BLOB (32)");

            entity.Property(e => e.Signature).HasColumnType("BLOB (256)");

            entity.Property(e => e.Source).HasColumnType("BLOB (32)");

            entity.HasOne(d => d.ChainHashNavigation)
                .WithMany(p => p.InverseChainHashNavigation)
                .HasPrincipalKey(p => p.Hash)
                .HasForeignKey(d => d.ChainHash)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SourceNavigation)
                .WithMany(p => p.Blocks)
                .HasPrincipalKey(p => p.Hash)
                .HasForeignKey(d => d.Source)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<KeyPair>(entity =>
        {
            entity.ToTable("KeyPair");

            entity.HasIndex(e => e.Hash, "IX_KeyPair_Hash")
                .IsUnique();

            entity.HasIndex(e => e.Private, "IX_KeyPair_Private")
                .IsUnique();

            entity.HasIndex(e => e.Public, "IX_KeyPair_Public")
                .IsUnique();

            entity.Property(e => e.Hash).HasColumnType("BLOB (32)");

            entity.Property(e => e.Private).HasColumnType("BLOB (1191)");

            entity.Property(e => e.Public).HasColumnType("BLOB (270)");
        });

        modelBuilder.Entity<Summary>(entity =>
        {
            entity.ToTable("Summary");

            entity.HasIndex(e => e.Hash, "IX_Summary_Hash")
                .IsUnique();

            entity.Property(e => e.Balance).HasColumnType("BIGINT");

            entity.Property(e => e.Hash).HasColumnType("BLOB (32)");

            entity.Property(e => e.Signature).HasColumnType("BLOB (256)");

            entity.Property(e => e.Source).HasColumnType("BLOB (32)");

            entity.Property(e => e.Target).HasColumnType("BLOB (32)");

            entity.HasOne(d => d.Block)
                .WithMany(p => p.Summaries)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SourceNavigation)
                .WithMany(p => p.SummarySourceNavigations)
                .HasPrincipalKey(p => p.Hash)
                .HasForeignKey(d => d.Source)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.TargetNavigation)
                .WithMany(p => p.SummaryTargetNavigations)
                .HasPrincipalKey(p => p.Hash)
                .HasForeignKey(d => d.Target)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transaction");

            entity.HasIndex(e => e.Hash, "IX_Transaction_Hash")
                .IsUnique();

            entity.Property(e => e.Amount).HasColumnType("BIGINT");

            entity.Property(e => e.Destination).HasColumnType("BLOB (32)");

            entity.Property(e => e.Hash).HasColumnType("BLOB (32)");

            entity.Property(e => e.Signature).HasColumnType("BLOB (256)");

            entity.Property(e => e.Source).HasColumnType("BLOB (32)");

            entity.HasOne(d => d.Block)
                .WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DestinationNavigation)
                .WithMany(p => p.TransactionDestinationNavigations)
                .HasPrincipalKey(p => p.Hash)
                .HasForeignKey(d => d.Destination)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SourceNavigation)
                .WithMany(p => p.TransactionSourceNavigations)
                .HasPrincipalKey(p => p.Hash)
                .HasForeignKey(d => d.Source)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
