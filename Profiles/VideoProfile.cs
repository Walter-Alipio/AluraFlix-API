using AutoMapper;

public class VideoProfile : Profile
{
  public VideoProfile()
  {
    CreateMap<CreateVideoDto, Video>();
  }
}