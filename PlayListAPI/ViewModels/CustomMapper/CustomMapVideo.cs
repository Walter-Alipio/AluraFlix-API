using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;

namespace PlayListAPI.ViewModels.CustomMapper;

public class CustomMapVideo : ICustomMapVideo
{

  public Video MapUpdateDtoToVideo(UpdateVideoDTO videoDTO, Video video)
  {
    if (!string.IsNullOrEmpty(videoDTO.Title))
    {
      video.Title = videoDTO.Title;
    }
    if (!string.IsNullOrEmpty(videoDTO.Description))
    {
      video.Description = videoDTO.Description;
    }
    if (!string.IsNullOrEmpty(videoDTO.Url))
    {
      video.Url = videoDTO.Url;
    }
    if (videoDTO.CategoriaId is not null or 0)
    {
      video.CategoriaId = (int)videoDTO.CategoriaId!;
    }

    return video;
  }
}