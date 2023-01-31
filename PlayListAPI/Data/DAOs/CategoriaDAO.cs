using Microsoft.EntityFrameworkCore;
using PlayListAPI.Data.DAOs.Interfaces;
using PlayListAPI.Models;

namespace PlayListAPI.Data.DAOs;

public class CategoriaDAO : ICategoriaDAO
{
  private AppDbContext _context;

  public CategoriaDAO(AppDbContext context)
  {
    _context = context;
  }

  public async Task AddAsync(Categoria categoria)
  {
    await _context.Categorias.AddAsync(categoria);
    await _context.SaveChangesAsync();
  }

  public async Task<List<Categoria>> GetCategoriasAsync()
  {
    return await _context.Categorias.ToListAsync();
  }

  public async Task<Categoria?> GetCategoriaByIdAsync(int id)
  {
    return await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);
  }

  public async Task<List<Video>> GetVideosByCategoriaAsync(int id)
  {
    return await _context.Videos
      .Include(v => v.Categoria)
      .Where(video => video.CategoriaId == id)
      .ToListAsync();
  }

  public async Task UpdateAsync(Categoria categoria)
  {
    _context.Categorias.Update(categoria);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(Categoria categoria)
  {
    _context.Remove(categoria);
    await _context.SaveChangesAsync();
  }

}