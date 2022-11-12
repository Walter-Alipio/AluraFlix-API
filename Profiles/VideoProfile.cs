using AluraPlayList.Data.DTOs.VideosDTOs;
using AluraPlayList.Models;
using AutoMapper;

public class VideoProfile : Profile
{
  public VideoProfile()
  {
    CreateMap<CreateVideoDto, Video>();
    CreateMap<Video, ReadVideoDTO>();
    CreateMap<UpdateVideoDTO, Video>().MapOnlyIfChanged();
  }
}