using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
  private IVideosService _videoService;

  public VideosController(IVideosService videoService)
  {
    _videoService = videoService;
  }

  /// <summary>
  /// Save a new Video.
  /// </summary>
  /// <returns></returns>
  /// <response code="201">If success</response>
  /// <response code="400">If new item data is incorrect</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  public IActionResult addVideo([FromBody] CreateVideoDto videoDto)
  {
    Result result = _videoService.addVideo(videoDto);
    if (result.IsFailed) return BadRequest(result.Errors.First());

    return Created("Video adicionado com sucesso!", result.Successes.FirstOrDefault());
  }

  /// <summary>
  /// Get Video by Id.
  /// </summary>
  /// <returns></returns>
  /// <response code="200">If success</response>
  /// <response code="404">If item id is null</response>
  [HttpGet("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public IActionResult showVideoById(int id)
  {
    ReadVideoDTO readDto = _videoService.ShowVideoById(id);
    if (readDto == null) return NotFound();

    return Ok(readDto);
  }


  /// <summary>
  /// Get all Videos.
  /// </summary>
  /// <returns></returns>
  /// <response code="200">If success</response>
  /// <response code="404">If item id is null</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public IActionResult showAllVideos([FromQuery] string? search)
  {
    List<ReadVideoDTO> readDtoList = _videoService.ShowAllVideos(search);
    if (readDtoList == null) return NotFound();

    return Ok(readDtoList);
  }

  /// <summary>
  /// Update a Video and return JSON with new data.
  /// </summary>
  /// <returns></returns>
  /// <response code="200">If success</response>
  /// <response code="404">If item id is null</response>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public IActionResult updateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    ReadVideoDTO selectedVideo = _videoService.IsValidId(id);
    if (selectedVideo == null) return NotFound();

    Result result = _videoService.ValidDTOFormat(videoDTO);

    if (result.IsFailed) return BadRequest(result.Errors.First());

    ReadVideoDTO readDto = _videoService.UpdateVideo(id, videoDTO);


    return CreatedAtAction(nameof(showVideoById), new { Id = readDto.Id }, readDto);
  }

  /// <summary>
  /// Delete Video.
  /// </summary>
  /// <returns></returns>
  /// <response code="204">If success</response>
  /// <response code="404">If item id is null</response>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public IActionResult deleteVideo(int id)
  {
    Result result = _videoService.DeleteVideo(id);
    if (result.IsFailed) return NotFound();

    return NoContent();
  }
}