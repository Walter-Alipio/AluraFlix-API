using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;

namespace PlayListAPI.ViewModels.CustomMapper;

public interface ICustomMapVideo
{
  Video MapUpdateDtoToVideo(UpdateVideoDTO videoDTO, Video video);
}
