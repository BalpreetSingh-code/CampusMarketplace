using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CampusMarketplace.Api.Data;

namespace CampusMarketplace.Api.Repositories;

//
// GenericRepository.cs — a reusable class for basic CRUD operations
//
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _ctx;  // Database context
    private readonly DbSet<T> _set;      // Represents the table for entity type T

    // Constructor: initializes the DbContext and DbSet
    public GenericRepository(AppDbContext ctx)
    {
        _ctx = ctx;
        _set = _ctx.Set<T>(); // Automatically sets the table based on the entity type
    }

    //
    // --- READ by ID ---
    // Finds a single entity by its primary key asynchronously
    //
    public async Task<T?> GetAsync(int id) => await _set.FindAsync(id);

    //
    // --- READ ALL / FILTERED ---
    // Returns all entities or only those matching a given filter expression
    //
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        => filter is null
            ? await _set.ToListAsync()        // No filter -> return all rows
            : await _set.Where(filter).ToListAsync(); // Apply filter if provided

    //
    // --- CREATE ---
    // Adds a new entity to the table asynchronously
    //
    public async Task AddAsync(T entity) => await _set.AddAsync(entity);

    //
    // --- UPDATE ---
    // Marks an existing entity as modified so changes will be saved
    //
    public void Update(T entity) => _set.Update(entity);

    //
    // --- DELETE ---
    // Removes an entity from the database set
    //
    public void Remove(T entity) => _set.Remove(entity);
}
