using Microsoft.EntityFrameworkCore;
using PlayListAPI.Data;
using PlayListAPI.Models;

namespace PlayListAPI.Repository.Handle;

public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
{
    private readonly AppDbContext _context;
    public CategoriaRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Video>> GetVideosByCategoriaAsync(int id)
    {
        return await _context.Videos
              .Include(v => v.Categoria)
              .Where(video => video.CategoriaId == id)
              .ToListAsync();
    }
}