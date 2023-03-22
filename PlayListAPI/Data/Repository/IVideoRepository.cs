using PlayListAPI.Models;

namespace PlayListAPI.Repository;

public interface IVideoRepository : IBaseRepository<Video>
{
  Task<List<Video>> GetAllUserVideos(string userId);
}
