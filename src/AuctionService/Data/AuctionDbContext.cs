using AuctionService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
        : base(options) { }

    public DbSet<Auction> Auctions { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Outbox table configuration
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        // Auction configuration
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Seller).IsRequired().HasMaxLength(100);

            entity.Property(a => a.Status).HasConversion<string>(); // store enum as string

            entity
                .HasOne(a => a.Item)
                .WithOne(i => i.Auction)
                .HasForeignKey<Item>(i => i.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Item configuration
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.Make).IsRequired().HasMaxLength(100);

            entity.Property(i => i.Model).IsRequired().HasMaxLength(100);

            entity.Property(i => i.Color).IsRequired().HasMaxLength(50);

            entity.Property(i => i.ImageUrl).HasMaxLength(500);
        });
    }
}
