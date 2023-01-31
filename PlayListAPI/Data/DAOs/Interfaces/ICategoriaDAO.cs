using PlayListAPI.Models;

namespace PlayListAPI.Data.DAOs.Interfaces;

public interface ICategoriaDAO
{
  Task AddAsync(Categoria categoria);
  Task DeleteAsync(Categoria categoria);
  Task<Categoria?> GetCategoriaByIdAsync(int id);
  Task<List<Categoria>> GetCategoriasAsync();
  Task<List<Video>> GetVideosByCategoriaAsync(int id);
  Task UpdateAsync(Categoria categoria);
}
