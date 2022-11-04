using AluraPlayList.Data.DTOs.CategoriasDTOs;
using AluraPlayList.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace AluraPlayList.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CategoriasController : ControllerBase
  {
    private CategoriasService _categoriaService;

    public CategoriasController(CategoriasService categoriaService)
    {
      _categoriaService = categoriaService;
    }

    [HttpPost]
    public IActionResult AddCategoria([FromBody] CreateCategoriasDto categoriaDto)
    {
      ReadCategoriasDto readCategoria = _categoriaService.AddCategoria(categoriaDto);
      if (readCategoria == null) return StatusCode(500, readCategoria);

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = readCategoria.Id }, readCategoria);
    }

    [HttpGet("{id}")]
    public IActionResult ShowCategoriaById(int id)
    {
      ReadCategoriasDto readCategoria = _categoriaService.ShowCategoriaById(id);
      if (readCategoria == null) return NotFound();

      return Ok(readCategoria);
    }

    [HttpGet("{id}/videos")]
    public IActionResult ShowVideosByCategoriaId(int id)
    {
      ReadCategoriaWithVideoDto readCategoria = _categoriaService.ShowVideosByCategoriaId(id);
      if (readCategoria == null) return NotFound();

      return Ok(readCategoria);
    }

    [HttpGet]
    public IActionResult ShowAllCategorias()
    {
      List<ReadCategoriasDto> categoriasDtos = _categoriaService.ShowAllCategorias();
      if (categoriasDtos == null) return NotFound();

      return Ok(categoriasDtos);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategoria(int id, [FromBody] UpdateCategoriasDtos updateDto)
    {
      ReadCategoriasDto categoriasDto = _categoriaService.UpdateCategoria(id, updateDto);
      if (categoriasDto == null) return NotFound();

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = categoriasDto.Id }, categoriasDto);
    }




    [HttpDelete("{id}")]
    public IActionResult DeleteCategorias(int id)
    {
      Result result = _categoriaService.DeleteCategorias(id);
      if (result.IsFailed) return NotFound();

      return NoContent();
    }
  }
}