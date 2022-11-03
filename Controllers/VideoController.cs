using System.Linq;
using System.Net;
using AluraPlayList.Services;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
  private IMapper _mapper;
  private AppDbContext _context;
  private VideosService _videoService;

  public VideosController(AppDbContext context, IMapper mapper, VideosService videoService = null)
  {
    _context = context;
    _mapper = mapper;
    _videoService = videoService;
  }

  [HttpPost]
  public IActionResult addVideo([FromBody] CreateVideoDto videoDto)
  {
    Result result = _videoService.addVideo(videoDto);
    if (result.IsFailed) return StatusCode(500, result.Errors.First());

    return StatusCode(201);
  }

  [HttpGet("{id}")]
  public IActionResult showVideoById(int id)
  {
    ReadVideoDTO readDto = _videoService.ShowVideoById(id);
    if (readDto == null) return NotFound();

    return Ok(readDto);
  }

  [HttpGet]
  public IActionResult showAllVideos()
  {
    List<ReadVideoDTO> readDtoList = _videoService.ShowAllVideos();
    if (readDtoList == null) return NotFound();

    return Ok(readDtoList);
  }

  [HttpPut("{id}")]
  public IActionResult updateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    ReadVideoDTO readDto = _videoService.UpdateVideo(id, videoDTO);
    if (videoDTO == null) return NotFound();

    return CreatedAtAction(nameof(showVideoById), new { Id = readDto.Id }, readDto);
  }



  [HttpDelete("{id}")]
  public IActionResult deleteVideo(int id)
  {
    Result result = _videoService.DeleteVideo(id);
    if (result.IsFailed) return NotFound();

    return NoContent();
  }
}