using FluentResults;
using PlayListAPI.Data.DTOs.CategoriasDTOs;
using PlayListAPI.Data.DTOs.VideosDTOs;

namespace PlayListAPI.Services.Interfaces;
public interface ICategoriasService
{
  ReadCategoriasDto AddCategoria(CreateCategoriasDto categoriaDto);
  Result DeleteCategorias(int id);
  List<ReadCategoriasDto> ShowAllCategorias();
  ReadCategoriasDto? ShowCategoriaById(int id);
  List<ReadVideoDTO> ShowVideosByCategoriaId(int id);
  ReadCategoriasDto UpdateCategoria(int id, UpdateCategoriasDtos updateCategoria);
}