using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Roles = "user")]
    public async Task<IActionResult> addVideo([FromBody] CreateVideoDto videoDto)
    {
        ReadVideoDTO? dto = await _videoService.AddVideoAsync(videoDto);
        if (dto == null) return BadRequest(dto);

        return CreatedAtAction(nameof(showVideoById), new { Id = dto.Id }, dto);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> showVideoById(int id)
    {
        ReadVideoDTO? readDto = await _videoService.GetVideoByIdAsync(id);
        if (readDto == null) return NotFound();

        return Ok(readDto);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> showAllVideos([FromQuery] string? search)
    {
        List<ReadVideoDTO>? readDtoList = await _videoService.GetVideosAsync(search);
        if (readDtoList == null || !readDtoList.Any()) return NotFound();

        return Ok(readDtoList);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> updateVideo(int id, [FromBody] UpdateVideoDTO videoDTO)
    {
        Result result = _videoService.CheckUrl(videoDTO);

        if (result.IsFailed) return BadRequest(result.Errors.First());

        ReadVideoDTO? selectedVideo = await _videoService.UpdateVideoAsync(id, videoDTO);
        if (selectedVideo == null) return NotFound();

        return CreatedAtAction(nameof(showVideoById), new { Id = selectedVideo.Id }, selectedVideo);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> deleteVideo(int id)
    {
        Result result = await _videoService.DeleteVideoAsync(id);
        if (result.IsFailed) return NotFound();

        return NoContent();
    }

}