using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PlayListAPI.Exceptions;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
  private readonly IVideoServiceUserData _videoService;
  private readonly ITokenExtract _tokenExtract;

  public VideosController(IVideoServiceUserData videoService, ITokenExtract tokenExtract)
  {
    _videoService = videoService;
    _tokenExtract = tokenExtract;
  }

  [HttpPost]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> AddVideo([FromBody] CreateVideoDto videoDto)
  {
    try
    {
      string userId = string.Empty;
      userId = _tokenExtract.ExtractID(Request.Headers["Authorization"]);

      ReadVideoDTO dto = await _videoService.AddVideoAsync(videoDto, userId);

      return CreatedAtAction(nameof(ShowVideoById), new { Id = dto.Id }, new
      {
        dto.Id,
        dto.Title,
        dto.Description,
        dto.Url,
        videoDto.CategoriaId
      });

    }
    catch (ErrorToGetUserIdException e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
    catch (ArgumentException e)
    {
      return StatusCode(StatusCodes.Status400BadRequest, e.Message);
    }

  }

  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<IActionResult> ShowVideoById(int id)
  {
    try
    {
      ReadVideoDTO readDto = await _videoService.GetVideoByIdAsync(id);

      return Ok(readDto);
    }
    catch (NullReferenceException e)
    {
      return StatusCode(StatusCodes.Status404NotFound, e.Message);
    }
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> ShowAllVideos([FromQuery] string? search)
  {
    List<ReadVideoDTO> readDtoList = await _videoService.GetVideosAsync(search);
    if (!readDtoList.Any()) return NotFound("Nenhum video encontrado.");

    return Ok(readDtoList);
  }

  [HttpGet("/Videos/bypage")]
  [AllowAnonymous]
  public async Task<IActionResult> ShowVideosPaginated(int page = 1, int pageSize = 5)
  {
    if (page <= 0) return NotFound();

    try
    {
      VideosPaginatedViewModel readList = await _videoService.GetPaginatedVideos(page, pageSize);

      return Ok(readList);
    }
    catch (NullReferenceException e)
    {
      return StatusCode(StatusCodes.Status404NotFound, e.Message);
    }

  }

  [HttpGet("/Meus_Videos")]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> ShowUserVideos([FromHeader(Name = "Authorization")] string authorizationHeader)
  {
    var userId = string.Empty;
    try
    {
      userId = _tokenExtract.ExtractID(authorizationHeader);
    }
    catch (ErrorToGetUserIdException e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
    List<ReadVideoDTO> readList = await _videoService.GetUserVideosAsync(userId);
    if (!readList.Any()) return NotFound();

    return Ok(readList);
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> UpdateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    try
    {

      ReadVideoDTO? selectedVideo;
      string userId = string.Empty;
      userId = _tokenExtract.ExtractID(Request.Headers["Authorization"]);

      selectedVideo = await _videoService.UpdateVideoAsync(id, videoDTO, userId);

      return NoContent();
    }
    catch (ErrorToGetUserIdException e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
    catch (ArgumentException e)
    {
      return BadRequest(e.Message);
    }
    catch (NullReferenceException e)
    {
      return StatusCode(StatusCodes.Status404NotFound, e.Message);
    }
    catch (NotTheOwnerException e)
    {
      return StatusCode(StatusCodes.Status405MethodNotAllowed, e.Message);
    }
    catch (Exception e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> DeleteVideo(int id)
  {
    try
    {

      string userId = string.Empty;
      userId = _tokenExtract.ExtractID(Request.Headers["Authorization"]);

      await _videoService.DeleteVideoAsync(id, userId);
      return NoContent();

    }
    catch (ErrorToGetUserIdException e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
    catch (NullReferenceException e)
    {
      return NotFound(e.Message);
    }
    catch (NotTheOwnerException e)
    {
      return StatusCode(StatusCodes.Status405MethodNotAllowed, e.Message);
    }

  }

}