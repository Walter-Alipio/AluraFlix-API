using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PlayListAPI.Data;
using PlayListAPI.Models;

namespace PlayListAPI.Repository.Handle;
public class BaseRepository<T> : IBaseRepository<T>
    where T : class, IEntity
{
  private readonly AppDbContext context;
  public BaseRepository(AppDbContext context)
  {
    this.context = context;
  }
  public async Task AddAsync(T obj)
  {
    await this.context.Set<T>().AddAsync(obj);
    await this.context.SaveChangesAsync();
  }

  public async Task<List<T>?> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<T, object>>? include = null)
  {
    var query = this.context.Set<T>().AsQueryable();

    if (include != null)
    {
      query = query.Include(include);
    }

    return await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();
  }

  public async Task Delete(T obj)
  {
    this.context.Remove(obj);
    await this.context.SaveChangesAsync();
  }

  public async Task<List<T>?> GetAll(Expression<Func<T, object>>? include = null)
  {
    var query = this.context.Set<T>().AsQueryable();

    if (include != null)
    {
      query = query.Include(include);
    }

    return await query.ToListAsync();
  }

  public async Task<T?> GetByIdAsync(int id, Expression<Func<T, object>>? include = null)
  {
    var query = this.context.Set<T>().AsQueryable();

    if (include != null)
    {
      query = query.Include(include);
    }

    return await query.FirstOrDefaultAsync(e => e.Id == id);
  }

  public async Task UpdateAsync(T obj)
  {
    this.context.Set<T>().Update(obj);
    await this.context.SaveChangesAsync();
  }

  public async Task<int> GetTotalItens()
  {
    return await this.context.Set<T>().CountAsync();
  }
}