
using Microsoft.EntityFrameworkCore;
using PlayListAPI.Data;
using PlayListAPI.Models;

namespace PlayListAPI.Repository.Handle;

public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
  private readonly AppDbContext _context;
  public VideoRepository(AppDbContext context) : base(context)
  {
    _context = context;
  }

  public async Task<List<Video>> GetAllUserVideos(string userId)
  {
    var queryList = await _context.Videos
      .Include(v => v.Categoria)
      .Where(video => video.AuthorId == userId).ToListAsync();
    return queryList;
  }
}