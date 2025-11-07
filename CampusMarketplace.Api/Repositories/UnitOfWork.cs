using CampusMarketplace.Api.Data;
using CampusMarketplace.Api.Models;

namespace CampusMarketplace.Api.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;
    public UnitOfWork(AppDbContext ctx)
    {
        _ctx = ctx;
        Categories = new GenericRepository<Category>(_ctx);
        Listings = new GenericRepository<Listing>(_ctx);
        Offers = new GenericRepository<Offer>(_ctx);
        Orders = new GenericRepository<Order>(_ctx);
        Reviews = new GenericRepository<Review>(_ctx);
    }
    public IGenericRepository<Category> Categories { get; }
    public IGenericRepository<Listing> Listings { get; }
    public IGenericRepository<Offer> Offers { get; }
    public IGenericRepository<Order> Orders { get; }
    public IGenericRepository<Review> Reviews { get; }
    public Task<int> SaveAsync() => _ctx.SaveChangesAsync();
}
