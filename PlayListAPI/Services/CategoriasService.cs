using PlayListAPI.Data;
using PlayListAPI.Data.DTOs.CategoriasDTOs;
using PlayListAPI.Models;
using AutoMapper;
using FluentResults;
using PlayListAPI.Data.DTOs.VideosDTOs;

namespace PlayListAPI.Services
{
  public class CategoriasService
  {
    private IMapper _mapper;
    private AppDbContext _context;

    public CategoriasService(IMapper mapper, AppDbContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    public ReadCategoriasDto AddCategoria(CreateCategoriasDto categoriaDto)
    {
      Categoria categoria = _mapper.Map<Categoria>(categoriaDto);

      _context.Categorias.Add(categoria);
      _context.SaveChanges();

      return _mapper.Map<ReadCategoriasDto>(categoria);
    }


    public ReadCategoriasDto ShowCategoriaById(int id)
    {
      Categoria? categoria = GetCategoriaById(id);

      if (categoria == null) return null;

      return _mapper.Map<ReadCategoriasDto>(categoria);
    }


    public List<ReadVideoDTO> ShowVideosByCategoriaId(int id)
    {
      List<Video>? videoCategoria = _context.Videos.Where(video => video.CategoriaId == id).ToList();

      if (!videoCategoria.Any()) return null;

      foreach (var video in videoCategoria)
      {
        video.Categoria = _context.Categorias.Where(categoria => video.CategoriaId == categoria.Id).FirstOrDefault();
        System.Console.WriteLine(video.Categoria);
      }

      return _mapper.Map<List<ReadVideoDTO>>(videoCategoria);
    }

    public List<ReadCategoriasDto> ShowAllCategorias()
    {
      List<Categoria> categorias = _context.Categorias.ToList();
      if (categorias == null) return null;

      return _mapper.Map<List<ReadCategoriasDto>>(categorias);
    }

    public ReadCategoriasDto UpdateCategoria(int id, UpdateCategoriasDtos updateCategoria)
    {
      Categoria? categoria = GetCategoriaById(id);
      if (categoria == null) return null;

      _mapper.Map(updateCategoria, categoria);
      _context.SaveChanges();

      return _mapper.Map<ReadCategoriasDto>(categoria);
    }



    public Result DeleteCategorias(int id)
    {
      Categoria? categoria = GetCategoriaById(id);
      if (categoria == null) return Result.Fail("Não encontrado");

      _context.Remove(categoria);
      _context.SaveChanges();
      return Result.Ok();
    }

    private Categoria? GetCategoriaById(int id)
    {
      return _context.Categorias.FirstOrDefault(categoria => categoria.Id == id);
    }
  }
}