using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CampusMarketplace.Api.Data;

namespace CampusMarketplace.Api.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _ctx;
    private readonly DbSet<T> _set;
    public GenericRepository(AppDbContext ctx) { _ctx = ctx; _set = _ctx.Set<T>(); }

    public async Task<T?> GetAsync(int id) => await _set.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        => filter is null ? await _set.ToListAsync() : await _set.Where(filter).ToListAsync();
    public async Task AddAsync(T entity) => await _set.AddAsync(entity);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);
}
