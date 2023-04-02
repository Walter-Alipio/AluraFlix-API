using PlayListAPI.Data.DTOs.VideosDTOs;
using FluentResults;

namespace PlayListAPI.Services.Interfaces
{
  public interface IVideosService
  {
    Task<ReadVideoDTO?> AddVideoAsync(CreateVideoDto videoDto, string userId);
    Task<Result> DeleteVideoAsync(int id);
    Task<List<ReadVideoDTO>?> GetVideosAsync(string? videoTitle);
    Task<ReadVideoDTO?> GetVideoByIdAsync(int id);
    Task<ReadVideoDTO?> UpdateVideoAsync(int id, UpdateVideoDTO videoDTO, string userId);
    Task<VideosPaginatedViewModel?> GetPaginatedVideos(int page, int pageSize);
  }
}