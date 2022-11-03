using System.Linq;
using System.Net;
using AluraPlayList.Data.DTOs.VideosDTOs;
using AluraPlayList.Services;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
  private VideosService _videoService;

  public VideosController(VideosService videoService = null)
  {
    _videoService = videoService;
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult addVideo([FromBody] CreateVideoDto videoDto)
  {
    Result result = _videoService.addVideo(videoDto);
    if (result.IsFailed) return StatusCode(500, result.Errors.First());

    return StatusCode(201);
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
  public IActionResult showAllVideos()
  {
    List<ReadVideoDTO> readDtoList = _videoService.ShowAllVideos();
    if (readDtoList == null) return NotFound();

    return Ok(readDtoList);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult updateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    ReadVideoDTO readDto = _videoService.UpdateVideo(id, videoDTO);
    if (videoDTO == null) return NotFound();

    return CreatedAtAction(nameof(showVideoById), new { Id = readDto.Id }, readDto);
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