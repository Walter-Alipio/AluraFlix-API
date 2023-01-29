using Microsoft.EntityFrameworkCore;
using PlayListAPI.Data;
using PlayListAPI.Data.DAOs.Interfaces;
using PlayListAPI.Models;

namespace PlayListAPI.Data.DAOs;

public class VideoDAO : IVideoDAO
{
  private AppDbContext _context;

  public VideoDAO(AppDbContext context)
  {
    _context = context;
  }
  //Database 
  public Task<List<Video>> GetVideos()
  {
    return _context.Videos
    .Include(v => v.Categoria)
    .ToListAsync();
  }
  public async Task<Video?> GetByIdAsync(int id)
  {
    return await _context.Videos
    .Include(v => v.Categoria)
    .FirstOrDefaultAsync(video => video.Id == id);
  }

  public async Task AddAsync(Video video)
  {
    await _context.Videos.AddAsync(video);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Video video)
  {
    _context.Videos.Update(video);
    await _context.SaveChangesAsync();
  }

  public async Task Delete(Video video)
  {
    _context.Remove(video);
    await _context.SaveChangesAsync();
  }

}