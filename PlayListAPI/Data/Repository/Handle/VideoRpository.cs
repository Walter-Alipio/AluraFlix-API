
using PlayListAPI.Data;
using PlayListAPI.Models;

namespace PlayListAPI.Repository.Handle;

public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
    public VideoRepository(AppDbContext context) : base(context)
    {
    }
}