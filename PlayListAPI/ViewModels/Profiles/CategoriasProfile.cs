using PlayListAPI.Models;
using PlayListAPI.Data.DTOs.CategoriasDTOs;
using AutoMapper;

namespace PlayListAPI.ViewModels.Profiles
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