using System.Linq.Expressions;

namespace CampusMarketplace.Api.Repositories;

//
// IGenericRepository.cs — defines common data access methods for any entity
//
public interface IGenericRepository<T> where T : class
{
    //
    // --- READ by ID ---
    // Finds a single entity based on its primary key (e.g., ID)
    //
    Task<T?> GetAsync(int id);

    //
    // --- READ ALL / FILTERED ---
    // Gets all records or filters them using a given condition (Expression<Func<T, bool>>)
    //
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);

    //
    // --- CREATE ---
    // Adds a new record to the database
    //
    Task AddAsync(T entity);

    //
    // --- UPDATE ---
    // Updates an existing record
    //
    void Update(T entity);

    //
    // --- DELETE ---
    // Removes a record from the database
    //
    void Remove(T entity);
}
