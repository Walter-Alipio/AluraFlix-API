using PlayListAPI.DTOs.CategoriasDTOs;
using PlayListAPI.Models;
using AutoMapper;
using FluentResults;
using PlayListAPI.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;
using System.Text.RegularExpressions;
using PlayListAPI.Repository;

namespace PlayListAPI.Services
{
  public class CategoriaService : ICategoriaService
  {
    private readonly IMapper _mapper;
    private readonly ICategoriaRepository _repository;

    public CategoriaService(IMapper mapper, ICategoriaRepository dao)
    {
      _mapper = mapper;
      _repository = dao;
    }

    public async Task<ReadCategoriasDto?> AddCategoriaAsync(CreateCategoriasDto categoriaDto)
    {
      if (string.IsNullOrEmpty(categoriaDto.Cor) || ColorPatternIsInvalid(categoriaDto.Cor))
      {
        return null;
      }

      Categoria categoria = _mapper.Map<Categoria>(categoriaDto);

      await _repository.AddAsync(categoria);

      return _mapper.Map<ReadCategoriasDto>(categoria);
    }

    public async Task<ReadCategoriasDto?> ShowCategoriaByIdAsync(int id)
    {
      Categoria? categoria = await _repository.GetByIdAsync(id, c => c.Videos);

      if (categoria is null) return null;

      return _mapper.Map<ReadCategoriasDto>(categoria);
    }

    public async Task<List<ReadVideoDTO>> ShowVideosByCategoriaIdAsync(int id)
    {
      List<Video> videoCategoria = await _repository.GetVideosByCategoriaAsync(id);

      if (!videoCategoria.Any()) return new List<ReadVideoDTO>();

      return _mapper.Map<List<ReadVideoDTO>>(videoCategoria);
    }

    public async Task<List<ReadCategoriasDto>> ShowAllCategoriasAsync()
    {
      List<Categoria>? categorias = await _repository.GetAll(c => c.Videos);
      if (categorias is null) return new List<ReadCategoriasDto>();

      return _mapper.Map<List<ReadCategoriasDto>>(categorias);
    }

    public async Task<ReadCategoriasDto?> UpdateCategoriaAsync(int id, UpdateCategoriasDtos updateCategoria)
    {
      Categoria? categoria = await _repository.GetByIdAsync(id, c => c.Videos);
      if (categoria is null) return null;

      _mapper.Map(updateCategoria, categoria);

      await _repository.UpdateAsync(categoria);

      return _mapper.Map<ReadCategoriasDto>(categoria);
    }

    public async Task DeleteCategoriasAsync(int id)
    {
      Categoria? categoria = await _repository.GetByIdAsync(id) ?? throw new NullReferenceException("Não encontrado");
      await _repository.Delete(categoria);
    }

    public Result? IsColorPatternValid(UpdateCategoriasDtos dtos)
    {
      if (string.IsNullOrEmpty(dtos.Cor)) return null;

      if (ColorPatternIsInvalid(dtos.Cor))
      {
        return Result.Fail("O campo cor deve seguir o padrão '#xxxxxx'");
      }

      return Result.Ok();
    }
    private static bool ColorPatternIsInvalid(string color)
    {
      color = color.ToLower();

      string pattern = @"^#?([\da-f]{6})$";
      Match match = Regex.Match(color, pattern, RegexOptions.IgnoreCase);

      if (!match.Success) return true;

      return false;
    }
  }
}