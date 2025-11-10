using CampusMarketplace.Api.Data;
using CampusMarketplace.Api.Models;

namespace CampusMarketplace.Api.Repositories;

//
// UnitOfWork.cs — coordinates all repositories and database changes
//
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx; // Shared database context used by all repositories

    // Constructor: initializes repositories and shares the same DbContext
    public UnitOfWork(AppDbContext ctx)
    {
        _ctx = ctx;

        // Each repository uses the same DbContext instance for consistency
        Categories = new GenericRepository<Category>(_ctx);
        Listings = new GenericRepository<Listing>(_ctx);
        Offers = new GenericRepository<Offer>(_ctx);
        Orders = new GenericRepository<Order>(_ctx);
        Reviews = new GenericRepository<Review>(_ctx);
    }

    // Repository properties for each entity
    public IGenericRepository<Category> Categories { get; }
    public IGenericRepository<Listing> Listings { get; }
    public IGenericRepository<Offer> Offers { get; }
    public IGenericRepository<Order> Orders { get; }
    public IGenericRepository<Review> Reviews { get; }

    //
    // --- SAVE CHANGES ---
    // Saves all changes made across repositories in one transaction
    //
    public Task<int> SaveAsync() => _ctx.SaveChangesAsync();
}
