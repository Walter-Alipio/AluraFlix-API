using PlayListAPI.Data.DTOs.CategoriasDTOs;
using PlayListAPI.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;

namespace PlayListAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CategoriasController : ControllerBase
  {
    private ICategoriasService _categoriaService;

    public CategoriasController(ICategoriasService categoriaService)
    {
      _categoriaService = categoriaService;
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddCategoria([FromBody] CreateCategoriasDto categoriaDto)
    {
      ReadCategoriasDto readCategoria = _categoriaService.AddCategoria(categoriaDto);
      if (readCategoria == null) return BadRequest(readCategoria);

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = readCategoria.Id }, readCategoria);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ShowCategoriaById(int id)
    {
      ReadCategoriasDto? readCategoria = _categoriaService.ShowCategoriaById(id);
      if (readCategoria == null) return NotFound();

      return Ok(readCategoria);
    }


    [HttpGet("{id}/videos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ShowVideosByCategoriaId(int id)
    {
      List<ReadVideoDTO> readVideoCategoria = _categoriaService.ShowVideosByCategoriaId(id);
      if (readVideoCategoria == null) return NotFound();

      return Ok(readVideoCategoria);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ShowAllCategorias()
    {
      List<ReadCategoriasDto> categoriasDtos = _categoriaService.ShowAllCategorias();
      if (!categoriasDtos.Any()) return NotFound();

      return Ok(categoriasDtos);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateCategoria(int id, [FromBody] UpdateCategoriasDtos updateDto)
    {
      ReadCategoriasDto categoriasDto = _categoriaService.UpdateCategoria(id, updateDto);
      if (categoriasDto == null) return NotFound();

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = categoriasDto.Id }, categoriasDto);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteCategorias(int id)
    {
      Result result = _categoriaService.DeleteCategorias(id);
      if (result.IsFailed) return NotFound();

      return NoContent();
    }
  }
}