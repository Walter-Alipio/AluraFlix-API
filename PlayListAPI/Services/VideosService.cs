using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;
using PlayListAPI.Services.Interfaces;
using AutoMapper;
using FluentResults;
using PlayListAPI.Repository;

namespace PlayListAPI.Services
{
  public class VideosService : IVideoServiceUserData
  {
    private readonly IMapper _mapper;
    private readonly IVideoRepository _repository;
    private readonly IConfiguration _configuration;

    public VideosService(IMapper mapper, IVideoRepository repository, IConfiguration configuration)
    {
      _mapper = mapper;
      _repository = repository;
      _configuration = configuration;
    }



    public async Task<ReadVideoDTO?> AddVideoAsync(CreateVideoDto videoDto, string userId)
    {
      Result resultado = CheckUrlPattern(videoDto);

      if (resultado.IsFailed)
      {
        return null;
      }

      Video video = _mapper.Map<Video>(videoDto);

      video.AuthorId = userId;
      video.CreatedAt = DateTime.Now;
      video.ModifyAt = DateTime.Now;

      await _repository.AddAsync(video);

      return _mapper.Map<ReadVideoDTO>(video);
    }


    public async Task<ReadVideoDTO?> GetVideoByIdAsync(int id)
    {
      Video? video = await _repository.GetByIdAsync(id, v => v.Categoria);
      if (video == null)
      {
        return null;
      }
      return _mapper.Map<ReadVideoDTO>(video);
    }


    public async Task<List<ReadVideoDTO>?> GetVideosAsync(string? videoTitle)
    {
      List<Video> videos = await _repository.GetAll(v => v.Categoria);
      if (!videos.Any())
      {
        return new List<ReadVideoDTO>();
      }

      if (!string.IsNullOrEmpty(videoTitle))
      {
        try
        {
          videos = videos.Where(v => v.Title.Contains(videoTitle)).ToList();
        }
        catch (NullReferenceException)
        {
          return new List<ReadVideoDTO>();
        }
      }

      return _mapper.Map<List<ReadVideoDTO>>(videos);
    }


    public async Task<ReadVideoDTO?> UpdateVideoAsync(int id, UpdateVideoDTO videoDTO)
    {
      if (videoDTO.Url is not null)
      {
        var result = this.CheckUrlPattern(videoDTO);

        if (result.IsFailed)
        {
          throw new ArgumentException(result.Errors.First().ToString());
        }
      }

      Video? video = await _repository.GetByIdAsync(id, v => v.Categoria)!;
      if (video == null)
      {
        throw new NullReferenceException();
      }

      _mapper.Map(videoDTO, video);

      video.ModifyAt = DateTime.Now;

      await _repository.UpdateAsync(video);

      return _mapper.Map<ReadVideoDTO>(video);
    }

    public async Task<Result> DeleteVideoAsync(int id)
    {
      Video? video = await _repository.GetByIdAsync(id);
      if (video is null)
      {
        return Result.Fail("Video não encontrado.");
      }

      await _repository.Delete(video);

      return Result.Ok();
    }

    public async Task<List<ReadVideoDTO>> GetUserVideosAsync(string userId)
    {
      List<Video> videos = await _repository.GetAllUserVideos(userId);
      if (!videos.Any())
      {
        return new List<ReadVideoDTO>();
      }

      return _mapper.Map<List<ReadVideoDTO>>(videos);
    }

    public async Task<VideosPaginated?> GetPaginatedVideos(int page, int pageSize)
    {
      List<Video>? videos = await _repository.GetAllPaginatedAsync(page, pageSize, v => v.Categoria);

      videos = videos is null ? new List<Video>() : videos;

      List<ReadVideoDTO> videosPage = _mapper.Map<List<ReadVideoDTO>>(videos);

      var total = await _repository.GetTotalItens();
      VideosPaginated? response = this.CreatePage(total, page, pageSize, videosPage);
      return response;
    }

    private VideosPaginated CreatePage(int total, int page, int pageSize, List<ReadVideoDTO> videosPage)
    {
      if (!videosPage.Any()) return null;

      var url = _configuration.GetValue<string>("url");

      int? previews = page - 1;

      var previewsLink = previews <= 0 ? null : $"{url}/Videos/bypage?page={previews}&pageSize={pageSize}";

      var nextLink = $"{url}/Videos/bypage?page={page + 1}&pageSize={pageSize}";

      var calcTotal = total - page * pageSize;

      if (calcTotal <= 0)
      {
        nextLink = null;
        calcTotal = 0;
      }

      return new VideosPaginated()
      {
        Total = calcTotal + " itens restantes",
        Next = nextLink,
        Previews = previewsLink,
        Videos = videosPage
      };
    }

    private Result CheckUrlPattern(VideoDto videoDto)
    {
      if (string.IsNullOrEmpty(videoDto.Url)) return Result.Fail("URL INVÁLIDA!");

      var urlDefault = "https://www.youtube.com/watch?v";

      string[] url = videoDto.Url.Split("=");

      if (!url[0].Equals(value: urlDefault) || url[1].Length != 11)
      {
        return Result.Fail("URL INVÁLIDA!");
      }
      return Result.Ok();
    }

  }
}
