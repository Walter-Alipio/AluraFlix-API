using PlayListAPI.Models;

namespace PlayListAPI.Repository;

public interface ICategoriaRepository : IBaseRepository<Categoria>
{
    Task<List<Video>> GetVideosByCategoriaAsync(int id);
}
