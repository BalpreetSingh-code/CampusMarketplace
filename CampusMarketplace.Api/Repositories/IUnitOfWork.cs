using CampusMarketplace.Api.Models;

namespace CampusMarketplace.Api.Repositories;
public interface IUnitOfWork
{
    IGenericRepository<Category> Categories { get; }
    IGenericRepository<Listing> Listings { get; }
    IGenericRepository<Offer> Offers { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<Review> Reviews { get; }
    Task<int> SaveAsync();
}
