using AluraPlayList.Models;
using AluraPlayList.Data.DTOs.CategoriasDTOs;
using AutoMapper;

namespace AluraPlayList.Profiles
{
  public class CategoriasProfile : Profile
  {
    public CategoriasProfile()
    {
      CreateMap<CreateCategoriasDto, Categoria>();
      CreateMap<Categoria, ReadCategoriasDto>();
      CreateMap<Categoria, ReadCategoriaWithVideoDto>()
        .ForMember(categoria => categoria.Videos, opts =>
          opts.MapFrom(categoria => categoria.Videos
            .Select(
              video => new { video.Id, video.Title, video.Url, video.Description }
            )));
      CreateMap<UpdateCategoriasDtos, Categoria>();
    }

  }
}