using PlayListAPI.Data;
using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;
using PlayListAPI.Services.Interfaces;
using AutoMapper;
using FluentResults;

namespace PlayListAPI.Services
{
  public class VideosService : IVideosService
  {
    private IMapper _mapper;
    private AppDbContext _context;
    private static string _URLCHECK = "https://www.youtube.com/watch?v";
    public VideosService(IMapper mapper, AppDbContext context)
    {
      _mapper = mapper;
      _context = context;
    }


    //POST new video
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

    //GET video by id
    public ReadVideoDTO? ShowVideoById(int id)
    {
      Video? video = GetVideoById(id);
      if (video == null)
      {
        return null;
      }
      video.Categoria = _context.Categorias.Where(categoria => video.CategoriaId == categoria.Id).FirstOrDefault();
      System.Console.WriteLine(video.Categoria);
      return _mapper.Map<ReadVideoDTO>(video);
    }

    //GET all videos
    public List<ReadVideoDTO>? ShowAllVideos(string? videoTitle)
    {
      List<Video> videos = _context.Videos.ToList();
      if (videos == null)
      {
        return null;
      }
      if (!string.IsNullOrEmpty(videoTitle))
      {
        try
        {
          IEnumerable<Video> query = from video in videos where video.Title.Contains(videoTitle) select video;
          videos = query.ToList();
        }
        catch
        {
          return null;
        }
      }


      foreach (var video in videos)
      {
        video.Categoria = _context.Categorias.Where(categoria => video.CategoriaId == categoria.Id).FirstOrDefault();
        System.Console.WriteLine(video.Categoria);
      }

      return _mapper.Map<List<ReadVideoDTO>>(videos);
    }

    //PUT update video information
    public ReadVideoDTO UpdateVideo(int id, UpdateVideoDTO videoDTO)
    {

      Video? video = GetVideoById(id)!;
      // if (video == null) return null;

      if (videoDTO.CategoriaId == 0)
        videoDTO.CategoriaId = video.CategoriaId;

      _mapper.Map(videoDTO, video);
      _context.SaveChanges();
      return _mapper.Map<ReadVideoDTO>(video);
    }

    //DELETE video from database
    public Result DeleteVideo(int id)
    {

      Video? video = GetVideoById(id);
      if (video == null)
      {
        return Result.Fail("Video não encontrado.");
      }
      _context.Remove(video);
      _context.SaveChanges();
      return Result.Ok();
    }



    //Checks if video url is a youtube valid url
    private Result urlTest(VideoDto videoDto)
    {
      if (string.IsNullOrEmpty(videoDto.Url)) return Result.Fail("URL INVÁLIDA!");

      string[] url = videoDto.Url.Split("=");
      if (!url[0].Equals(value: _URLCHECK) || url[1].Length != 11)
      {
        return Result.Fail("URL INVÁLIDA!");
      }
      return Result.Ok();
    }

    public ReadVideoDTO? IsValidId(int id)
    {
      Video? video = GetVideoById(id);
      if (video == null)
      {
        return null;
      }

      return _mapper.Map<ReadVideoDTO>(video);
    }

    public Result ValidDTOFormat(UpdateVideoDTO videoDTO)
    {
      try
      {
        Result result;
        if (!String.IsNullOrEmpty(videoDTO.Url))
        {
          result = urlTest(videoDTO);
          if (result.IsFailed)
          {
            throw new Exception(result.Errors.First().ToString());
          }
        }
      }
      catch (Exception e)
      {
        System.Console.WriteLine(e.Message);
        System.Console.WriteLine(e.StackTrace);
        return Result.Fail(e.Message);
      }

      return Result.Ok();
    }
    //Search in database 
    private Video? GetVideoById(int id)
    {
      return _context.Videos.FirstOrDefault(video => video.Id == id);
    }


  }
}
