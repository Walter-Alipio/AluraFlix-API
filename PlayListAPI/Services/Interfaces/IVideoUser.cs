using PlayListAPI.DTOs.VideosDTOs;

namespace PlayListAPI.Services.Interfaces;
public interface IVideoServiceUserData : IVideosService
{
  Task<List<ReadVideoDTO>> GetUserVideosAsync(string userId);
}