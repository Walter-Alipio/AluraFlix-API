using PlayListAPI.Data.DTOs.VideosDTOs;
using FluentResults;

namespace PlayListAPI.Services.Interfaces
{
  public interface IVideosService
  {
    Task<Result> AddVideoAsync(CreateVideoDto videoDto);
    Task<Result> DeleteVideoAsync(int id);
    Task<List<ReadVideoDTO>?> GetVideosAsync(string? videoTitle);
    Task<ReadVideoDTO?> GetVideoByIdAsync(int id);
    Task<ReadVideoDTO> UpdateVideoAsync(int id, UpdateVideoDTO videoDTO);
    Result CheckUrl(UpdateVideoDTO videoDTO);
  }
}