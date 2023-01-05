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
  public IActionResult addVideo([FromBody] CreateVideoDto videoDto)
  {
    Result result = _videoService.addVideo(videoDto);
    if (result.IsFailed) return BadRequest(result.Errors.First());

    return Created("Video adicionado com sucesso!", result.Successes.FirstOrDefault());
  }


  [HttpGet("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult showVideoById(int id)
  {
    ReadVideoDTO readDto = _videoService.ShowVideoById(id);
    if (readDto == null) return NotFound();

    return Ok(readDto);
  }



  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult showAllVideos([FromQuery] string? search)
  {
    List<ReadVideoDTO> readDtoList = _videoService.ShowAllVideos(search);
    if (!readDtoList.Any()) return NotFound();

    return Ok(readDtoList);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult updateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    ReadVideoDTO? selectedVideo = _videoService.IsValidId(id);
    if (selectedVideo == null) return NotFound();

    Result result = _videoService.ValidDTOFormat(videoDTO);

    if (result.IsFailed) return BadRequest(result.Errors.First());

    selectedVideo = _videoService.UpdateVideo(id, videoDTO);


    return CreatedAtAction(nameof(showVideoById), new { Id = selectedVideo.Id }, selectedVideo);
  }


  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult deleteVideo(int id)
  {
    Result result = _videoService.DeleteVideo(id);
    if (result.IsFailed) return NotFound();

    return NoContent();
  }
}