using AutoMapper;

public class VideoProfile : Profile
{
  public VideoProfile()
  {
    CreateMap<CreateVideoDto, Video>();
    CreateMap<Video, ReadVideoDTO>();
  }
}