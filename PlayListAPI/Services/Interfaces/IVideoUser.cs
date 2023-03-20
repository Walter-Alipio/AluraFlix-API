using PlayListAPI.Data.DTOs.VideosDTOs;

namespace PlayListAPI.Services.Interfaces;
public interface IVideoUser : IVideosService
{
  Task<List<ReadVideoDTO>> ShowUserVideosAsync(string userId);
}