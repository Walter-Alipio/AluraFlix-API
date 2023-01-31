using FluentResults;
using PlayListAPI.Data.DTOs.CategoriasDTOs;
using PlayListAPI.Data.DTOs.VideosDTOs;

namespace PlayListAPI.Services.Interfaces;
public interface ICategoriaService
{
  Task<ReadCategoriasDto?> AddCategoriaAsync(CreateCategoriasDto categoriaDto);
  Task<Result> DeleteCategoriasAsync(int id);
  Task<List<ReadCategoriasDto>> ShowAllCategoriasAsync();
  Task<ReadCategoriasDto?> ShowCategoriaByIdAsync(int id);
  Task<List<ReadVideoDTO>> ShowVideosByCategoriaIdAsync(int id);
  Task<ReadCategoriasDto?> UpdateCategoriaAsync(int id, UpdateCategoriasDtos updateCategoria);
  Result? IsColorPatternValid(UpdateCategoriasDtos dtos);
}