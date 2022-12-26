using PlayListAPI.Data.DTOs.CategoriasDTOs;
using PlayListAPI.Services;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace PlayListAPI.Controllers
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

    /// <summary>
    /// Save a new Categoria.
    /// </summary>
    /// <returns></returns>
    /// <response code="201">If success</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddCategoria([FromBody] CreateCategoriasDto categoriaDto)
    {
      ReadCategoriasDto readCategoria = _categoriaService.AddCategoria(categoriaDto);
      if (readCategoria == null) return StatusCode(500, readCategoria);

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = readCategoria.Id }, readCategoria);
    }

    /// <summary>
    /// Get Categoria by Id.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">If success</response>
    /// <response code="404">If item id is null</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ShowCategoriaById(int id)
    {
      ReadCategoriasDto readCategoria = _categoriaService.ShowCategoriaById(id);
      if (readCategoria == null) return NotFound();

      return Ok(readCategoria);
    }

    /// <summary>
    /// Get Videos by Categoria Id.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">If success</response>
    /// <response code="404">If item id is null</response>
    [HttpGet("{id}/videos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ShowVideosByCategoriaId(int id)
    {
      ReadCategoriaWithVideoDto readCategoria = _categoriaService.ShowVideosByCategoriaId(id);
      if (readCategoria == null) return NotFound();

      return Ok(readCategoria);
    }

    /// <summary>
    /// Get all Categorias.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">If success</response>
    /// <response code="404">If item id is null</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ShowAllCategorias()
    {
      List<ReadCategoriasDto> categoriasDtos = _categoriaService.ShowAllCategorias();
      if (categoriasDtos == null) return NotFound();

      return Ok(categoriasDtos);
    }

    /// <summary>
    /// Update a Categoria and return JSON with new data.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">If success</response>
    /// <response code="404">If item id is null</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult UpdateCategoria(int id, [FromBody] UpdateCategoriasDtos updateDto)
    {
      ReadCategoriasDto categoriasDto = _categoriaService.UpdateCategoria(id, updateDto);
      if (categoriasDto == null) return NotFound();

      return CreatedAtAction(nameof(ShowCategoriaById), new { Id = categoriasDto.Id }, categoriasDto);
    }

    /// <summary>
    /// Delete Categoria.
    /// </summary>
    /// <returns></returns>
    /// <response code="204">If success</response>
    /// <response code="404">If item id is null</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteCategorias(int id)
    {
      Result result = _categoriaService.DeleteCategorias(id);
      if (result.IsFailed) return NotFound();

      return NoContent();
    }
  }
}