using PlayListAPI.Data.DTOs.VideosDTOs;
using FluentResults;

namespace PlayListAPI.Services.Interfaces
{
  public interface IVideosService
  {
    Result addVideo(CreateVideoDto videoDto);
    Result DeleteVideo(int id);
    ReadVideoDTO? IsValidId(int id);
    List<ReadVideoDTO>? ShowAllVideos(string? videoTitle);
    ReadVideoDTO? ShowVideoById(int id);
    ReadVideoDTO UpdateVideo(int id, UpdateVideoDTO videoDTO);
    Result ValidDTOFormat(UpdateVideoDTO videoDTO);
  }
}