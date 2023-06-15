using PlayListAPI.DTOs.VideosDTOs;
using PlayListAPI.Models;
using AutoMapper;

namespace PlayListAPI.Profiles;
public class VideoProfile : Profile
{
  public VideoProfile()
  {
    CreateMap<CreateVideoDto, Video>();
    CreateMap<Video, ReadVideoDTO>();
    // CreateMap<UpdateVideoDTO, Video>();
  }

}