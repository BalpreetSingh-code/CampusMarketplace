using CampusMarketplace.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusMarketplace.Api.Data;

/// <summary>
/// Represents the EF Core database context for the Campus Marketplace app.
/// Manages all tables (DbSets) and relationships.
/// </summary>
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets tell EF Core to create these tables in the database
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Prevent multiple cascade delete issues
        builder.Entity<Offer>()
            .HasOne(o => o.Listing)
            .WithMany(l => l.Offers)
            .HasForeignKey(o => o.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Listing)
            .WithMany(l => l.Orders)
            .HasForeignKey(o => o.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure one-to-one between Order and Review
        builder.Entity<Order>()
            .HasOne(o => o.Review)
            .WithOne(r => r.Order)
            .HasForeignKey<Review>(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent cascade delete loops between reviewers
        builder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Review>()
            .HasOne(r => r.Reviewee)
            .WithMany()
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
