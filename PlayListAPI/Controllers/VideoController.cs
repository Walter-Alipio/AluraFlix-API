using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
  private IVideoServiceUserData _videoService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public VideosController(IVideoServiceUserData videoService, IHttpContextAccessor httpcontextAccessor)
  {
    _videoService = videoService;
    _httpContextAccessor = httpcontextAccessor;
  }

  [HttpPost]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> AddVideo([FromBody] CreateVideoDto videoDto)
  {
    var userId = string.Empty;

    try
    {
      userId = ExtractId();
    }
    catch (System.Exception e)
    {
      return StatusCode(500, $" {e.Message}");
    }

    ReadVideoDTO? dto = await _videoService.AddVideoAsync(videoDto, userId);
    if (dto == null) return BadRequest();

    return CreatedAtAction(nameof(ShowVideoById), new { Id = dto.Id }, dto);
  }

  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<IActionResult> ShowVideoById(int id)
  {
    ReadVideoDTO? readDto = await _videoService.GetVideoByIdAsync(id);
    if (readDto == null) return NotFound();

    return Ok(readDto);
  }

  [HttpGet]
  [AllowAnonymous]
  public async Task<IActionResult> ShowAllVideos([FromQuery] string? search)
  {
    List<ReadVideoDTO>? readDtoList = await _videoService.GetVideosAsync(search);
    if (readDtoList == null || !readDtoList.Any()) return NotFound();

    return Ok(readDtoList);
  }

  [HttpGet("/Videos/bypage")]
  [AllowAnonymous]
  public async Task<IActionResult> ShowVideosPaginated(int page = 1, int pageSize = 5)
  {
    if (page <= 0) return NotFound();

    var readList = await _videoService.GetPaginatedVideos(page, pageSize);
    if (readList == null) return NotFound();

    return Ok(readList);
  }

  [HttpGet("/Meus_Videos")]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> ShowUserVideos()
  {
    var userId = string.Empty;

    try
    {
      userId = ExtractId();
    }
    catch (System.Exception e)
    {
      return StatusCode(500, $" {e.Message}");
    }
    List<ReadVideoDTO> readList = await _videoService.GetUserVideosAsync(userId);
    if (readList is null || !readList.Any()) return NotFound();

    return Ok(readList);
  }


  [HttpPut("{id}")]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> UpdateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    ReadVideoDTO selectedVideo;
    try
    {
      selectedVideo = await _videoService.UpdateVideoAsync(id, videoDTO);
    }
    catch (ArgumentException e)
    {
      return BadRequest(e.Message);
    }
    catch (NullReferenceException)
    {
      return NotFound();
    }

    return CreatedAtAction(nameof(ShowVideoById), new { Id = selectedVideo.Id }, selectedVideo);
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "user")]
  public async Task<IActionResult> DeleteVideo(int id)
  {
    Result result = await _videoService.DeleteVideoAsync(id);
    if (result.IsFailed) return NotFound();

    return NoContent();
  }
  private string ExtractId()
  {
    try
    {
      var authHeader = Request.Headers["Authorization"];

      var token = authHeader.First().Substring("Bearer ".Length).Trim();

      // Extrair as reivindicações do token JWT
      var handler = new JwtSecurityTokenHandler();
      var claims = handler.ReadJwtToken(token).Claims;

      var userIdClaim = claims.FirstOrDefault(c => c.Type == "id");

      return userIdClaim.Value;
    }
    catch (System.Exception)
    {
      throw;
    }
  }

}