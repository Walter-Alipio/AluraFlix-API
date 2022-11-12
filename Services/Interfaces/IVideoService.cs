using AluraPlayList.Data.DTOs.VideosDTOs;
using FluentResults;

namespace AluraPlayList.Services.Interfaces
{
  public interface IVideosService
  {
    Result addVideo(CreateVideoDto videoDto);
    Result DeleteVideo(int id);
    ReadVideoDTO IsValidId(int id);
    List<ReadVideoDTO> ShowAllVideos(string? videoTitle);
    ReadVideoDTO ShowVideoById(int id);
    ReadVideoDTO UpdateVideo(int id, UpdateVideoDTO videoDTO);
    Result ValidDTOFormat(UpdateVideoDTO videoDTO);
  }
}