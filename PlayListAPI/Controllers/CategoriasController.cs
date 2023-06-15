using PlayListAPI.DTOs.CategoriasDTOs;
using Microsoft.AspNetCore.Mvc;
using PlayListAPI.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PlayListAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class CategoriasController : ControllerBase
  {
    private ICategoriaService _categoriaService;
    public CategoriasController(ICategoriaService categoriaService)
    {
      _categoriaService = categoriaService;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddCategoria([FromBody] CreateCategoriasDto categoriaDto)
    {
      ReadCategoriasDto? readCategoria = await _categoriaService.AddCategoriaAsync(categoriaDto);

      if (readCategoria == null) return BadRequest(readCategoria);

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = readCategoria.Id }, readCategoria);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> ShowCategoriaById(int id)
    {
      ReadCategoriasDto? readCategoria = await _categoriaService.ShowCategoriaByIdAsync(id);

      if (readCategoria == null) return NotFound();

      return Ok(readCategoria);
    }

    [HttpGet("{id}/videos")]
    [AllowAnonymous]
    public async Task<IActionResult> ShowVideosByCategoriaId(int id)
    {
      List<ReadVideoDTO> readVideoCategoria = await _categoriaService.ShowVideosByCategoriaIdAsync(id);

      if (!readVideoCategoria.Any()) return NotFound();

      return Ok(readVideoCategoria);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ShowAllCategorias()
    {
      List<ReadCategoriasDto> categoriasDtos = await _categoriaService.ShowAllCategoriasAsync();
      if (!categoriasDtos.Any()) return NotFound("Nenhuma categoria encontrada");
      return Ok(categoriasDtos);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateCategoria(int id, [FromBody] UpdateCategoriasDtos updateDto)
    {
      ReadCategoriasDto? categoriasDto = await _categoriaService.UpdateCategoriaAsync(id, updateDto);

      if (categoriasDto == null) return NotFound();

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = categoriasDto.Id }, categoriasDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteCategorias(int id)
    {
      try
      {
        await _categoriaService.DeleteCategoriasAsync(id);
        return NoContent();
      }
      catch (NullReferenceException e)
      {
        System.Console.WriteLine(e.Message);
        System.Console.WriteLine(e.StackTrace);
        return NotFound();
      }
      catch (System.Exception)
      {
        throw;
      }
    }
  }
}