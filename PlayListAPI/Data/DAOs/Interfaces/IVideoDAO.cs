using PlayListAPI.Models;

namespace PlayListAPI.Data.DAOs.Interfaces;

public interface IVideoDAO
{
  Task AddAsync(Video video);
  Task Delete(Video video);
  Task<Video?> GetByIdAsync(int id);
  Task<List<Video>> GetVideos();
  Task UpdateAsync(Video video);
}
