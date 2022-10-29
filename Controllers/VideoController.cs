using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class VideosController : ControllerBase
{
  private IMapper _mapper;
  private AppDbContext _context;

  public VideosController(AppDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }


  [HttpPost]
  public IActionResult addVideo([FromBody] CreateVideoDto videoDto)
  {
    try
    {
      videoDto.urlTest();
      Video video = _mapper.Map<Video>(videoDto);
      _context.Videos.Add(video);
      _context.SaveChanges();

      return CreatedAtAction(nameof(showVideoById), new { Id = video.Id }, video);
    }
    catch (System.Exception e)
    {

      return BadRequest(e.Message);
    }
  }

  [HttpGet("{id}")]
  public IActionResult showVideoById(int id)
  {
    Video video = _context.Videos.FirstOrDefault(video => video.Id == id);
    if (video == null)
    {
      return NotFound();
    }
    ReadVideoDTO videoDTO = _mapper.Map<ReadVideoDTO>(video);
    return Ok(videoDTO);
  }

  [HttpGet]
  public IActionResult showAllVideo()
  {
    List<Video> videos = _context.Videos.ToList();
    if (videos == null)
    {
      return NotFound();
    }
    List<ReadVideoDTO> readDto = _mapper.Map<List<ReadVideoDTO>>(videos);

    return Ok(readDto);
  }

  [HttpPut("{id}")]
  public IActionResult updateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
  {
    Video video = _context.Videos.FirstOrDefault(video => video.Id == id);
    if (video == null)
    {
      return NotFound();
    }

    _mapper.Map(videoDTO, video);
    _context.SaveChanges();
    return NoContent();
  }



  [HttpDelete("{id}")]
  public IActionResult deleteVideo(int id)
  {
    Video video = _context.Videos.FirstOrDefault(video => video.Id == id);
    if (video == null)
    {
      return NotFound();
    }

    _context.Remove(video);
    _context.SaveChanges();
    return NoContent();
  }
}