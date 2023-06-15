using PlayListAPI.DTOs.VideosDTOs;
using PlayListAPI.Models;

namespace PlayListAPI.Profiles.CustomMapper;

public interface ICustomMapVideo
{
  Video MapUpdateDtoToVideo(UpdateVideoDTO videoDTO, Video video);
}
