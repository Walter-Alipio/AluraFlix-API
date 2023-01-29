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

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> addVideo([FromBody] CreateVideoDto videoDto)
  {
    Result result = await _videoService.AddVideoAsync(videoDto);
    if (result.IsFailed) return BadRequest(result.Errors.First());

    return Created("Video adicionado com sucesso!", result.Successes.FirstOrDefault());
  }


  [HttpGet("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> showVideoById(int id)
  {
    ReadVideoDTO? readDto = await _videoService.GetVideoByIdAsync(id);
    if (readDto == null) return NotFound();

    return Ok(readDto);
  }


  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> showAllVideos([FromQuery] string? search)
  {
    List<ReadVideoDTO>? readDtoList = await _videoService.GetVideosAsync(search);
    if (readDtoList == null || !readDtoList.Any()) return NotFound();

    return Ok(readDtoList);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> updateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    ReadVideoDTO? selectedVideo = await _videoService.GetVideoByIdAsync(id);
    if (selectedVideo == null) return NotFound();

    Result result = _videoService.CheckUrl(videoDTO);

    if (result.IsFailed) return BadRequest(result.Errors.First());

    selectedVideo = await _videoService.UpdateVideoAsync(id, videoDTO);


    return CreatedAtAction(nameof(showVideoById), new { Id = selectedVideo.Id }, selectedVideo);
  }


  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> deleteVideo(int id)
  {
    Result result = await _videoService.DeleteVideoAsync(id);
    if (result.IsFailed) return NotFound();

    return NoContent();
  }
}