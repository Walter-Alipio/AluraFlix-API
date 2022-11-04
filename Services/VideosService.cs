using AluraPlayList.Data;
using AluraPlayList.Data.DTOs.VideosDTOs;
using AluraPlayList.Models;
using AutoMapper;
using FluentResults;

namespace AluraPlayList.Services
{
  public class VideosService
  {
    private IMapper _mapper;
    private AppDbContext _context;
    private string _urlCheck = "https://www.youtube.com/watch?v";
    public VideosService(IMapper mapper, AppDbContext context)
    {
      _mapper = mapper;
      _context = context;
    }

    public Result addVideo(CreateVideoDto videoDto)
    {
      Result resultado = urlTest(videoDto);
      if (videoDto.CategoriaId == 0)
      {
        videoDto.CategoriaId = 1;
      }

      if (resultado.IsFailed) return resultado;

      Video video = _mapper.Map<Video>(videoDto);
      _context.Videos.Add(video);
      _context.SaveChanges();

      ReadVideoDTO readDto = _mapper.Map<ReadVideoDTO>(video);
      return Result.Ok();
    }

    private Result urlTest(CreateVideoDto videoDto)
    {
      string[] url = videoDto.Url.Split("=");
      if (!url[0].Equals(value: _urlCheck) || url[1].Length != 11)
      {
        return Result.Fail("URL INVÁLIDA!");
      }
      return Result.Ok();
    }

    public ReadVideoDTO ShowVideoById(int id)
    {
      Video? video = _context.Videos.FirstOrDefault(video => video.Id == id);
      if (video == null)
      {
        return null;
      }
      return _mapper.Map<ReadVideoDTO>(video);
    }

    public List<ReadVideoDTO> ShowAllVideos(string? videoTitle)
    {
      List<Video> videos = _context.Videos.ToList();
      if (videos == null)
      {
        return null;
      }
      if (videoTitle != null)
      {
        IEnumerable<Video> query = from video in videos where video.Title == videoTitle select video;
        videos = query.ToList();
      }

      return _mapper.Map<List<ReadVideoDTO>>(videos);
    }

    public ReadVideoDTO UpdateVideo(int id, UpdateVideoDTO videoDTO)
    {
      Video? video = _context.Videos.FirstOrDefault(video => video.Id == id);
      if (video == null)
      {
        return null;
      }
      _mapper.Map(videoDTO, video);
      _context.SaveChanges();
      return _mapper.Map<ReadVideoDTO>(video);
    }

    public Result DeleteVideo(int id)
    {

      Video? video = _context.Videos.FirstOrDefault(video => video.Id == id);
      if (video == null)
      {
        return Result.Fail("Video não encontrado.");
      }
      _context.Remove(video);
      _context.SaveChanges();
      return Result.Ok();
    }
  }
}
