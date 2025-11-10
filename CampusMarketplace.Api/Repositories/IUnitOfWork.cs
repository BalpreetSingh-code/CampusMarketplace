using CampusMarketplace.Api.Models;

namespace CampusMarketplace.Api.Repositories;

//
// IUnitOfWork.cs — defines a single access point for all repositories
//
public interface IUnitOfWork
{
    //
    // Repositories for different entities
    //
    IGenericRepository<Category> Categories { get; } // Handles Category table operations
    IGenericRepository<Listing> Listings { get; }    // Handles Listing table operations
    IGenericRepository<Offer> Offers { get; }        // Handles Offer table operations
    IGenericRepository<Order> Orders { get; }        // Handles Order table operations
    IGenericRepository<Review> Reviews { get; }      // Handles Review table operations

    //
    // --- SAVE CHANGES ---
    // Commits all pending changes to the database in one transaction
    //
    Task<int> SaveAsync();
}
