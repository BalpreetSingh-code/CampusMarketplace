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
    // Constructor: passes DbContext options to the base class
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //
    // --- Tables (DbSets) ---
    // Each DbSet represents a table in the database
    //
    public DbSet<Category> Categories => Set<Category>();  // Stores all categories
    public DbSet<Listing> Listings => Set<Listing>();      // Stores listings posted by users
    public DbSet<Offer> Offers => Set<Offer>();            // Stores buyer offers
    public DbSet<Order> Orders => Set<Order>();            // Stores confirmed purchases
    public DbSet<Review> Reviews => Set<Review>();         // Stores user reviews after orders

    //
    // --- Model Configuration ---
    // Defines how entities relate to each other and handles delete behaviors
    //
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Prevent cascade delete conflicts between Listing and Offer
        builder.Entity<Offer>()
            .HasOne(o => o.Listing)
            .WithMany(l => l.Offers)
            .HasForeignKey(o => o.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent cascade delete conflicts between Listing and Order
        builder.Entity<Order>()
            .HasOne(o => o.Listing)
            .WithMany(l => l.Orders)
            .HasForeignKey(o => o.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Define one-to-one relationship between Order and Review
        builder.Entity<Order>()
            .HasOne(o => o.Review)
            .WithOne(r => r.Order)
            .HasForeignKey<Review>(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent cascade delete loops between reviewer and reviewee
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
