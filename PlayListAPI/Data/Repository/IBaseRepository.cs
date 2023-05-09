using System.Linq.Expressions;
namespace PlayListAPI.Repository;
public interface IBaseRepository<T>
{
  Task AddAsync(T obj);
  Task Delete(T obj);
  Task<T?> GetByIdAsync(int id, Expression<Func<T, object>>? include = null);
  Task<List<T>?> GetAll(Expression<Func<T, object>>? include = null);
  Task UpdateAsync(T obj);
  Task<List<T>?> GetAllPaginatedAsync(int page, int pageSize, Expression<Func<T, object>>? include = null);
  Task<int> GetTotalItens();

}