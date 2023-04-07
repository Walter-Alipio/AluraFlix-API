using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;
using PlayListAPI.Services.Interfaces;
using AutoMapper;
using FluentResults;
using PlayListAPI.Repository;
using PlayListAPI.ViewModels;
using PlayListAPI.Exceptions;

namespace PlayListAPI.Services
{
  public class VideosService : IVideoServiceUserData
  {
    private readonly IMapper _mapper;
    private readonly IVideoRepository _repository;

    public VideosService(IMapper mapper, IVideoRepository repository)
    {
      _mapper = mapper;
      _repository = repository;
    }

    public async Task<ReadVideoDTO> AddVideoAsync(CreateVideoDto videoDto, string userId)
    {
      Result resultado = CheckUrlPattern(videoDto);

      if (resultado.IsFailed)
      {
        throw new ArgumentException("Formato de url inválido");
      }

      Video video = _mapper.Map<Video>(videoDto);

      video.AuthorId = userId;
      video.CreatedAt = DateTime.Now;
      video.ModifyAt = DateTime.Now;

      await _repository.AddAsync(video);

      return _mapper.Map<ReadVideoDTO>(video);
    }


    public async Task<ReadVideoDTO> GetVideoByIdAsync(int id)
    {
      Video? video = await _repository.GetByIdAsync(id, v => v.Categoria);
      if (video == null)
      {
        throw new NullReferenceException("Video não encontrado.");
      }
      return _mapper.Map<ReadVideoDTO>(video);
    }


    public async Task<List<ReadVideoDTO>> GetVideosAsync(string? videoTitle)
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


    public async Task<ReadVideoDTO> UpdateVideoAsync(int id, UpdateVideoDTO videoDTO, string userId)
    {

      Video? video = await _repository.GetByIdAsync(id, v => v.Categoria)!;
      if (video == null)
      {
        throw new NullReferenceException();
      }

      if (!userId.Equals(video.AuthorId))
      {
        throw new NotTheOwnerException("Você não é dono deste video.");
      }


      if (videoDTO.Url is not null)
      {
        var result = this.CheckUrlPattern(videoDTO);

        if (result.IsFailed)
        {
          throw new ArgumentException(result.Errors.First().ToString());
        }
      }


      _mapper.Map(videoDTO, video);

      video.ModifyAt = DateTime.Now;

      await _repository.UpdateAsync(video);

      return _mapper.Map<ReadVideoDTO>(video);
    }

    public async Task DeleteVideoAsync(int id, string userId)
    {
      Video? video = await _repository.GetByIdAsync(id);
      if (video is null)
      {
        throw new NullReferenceException("Video não encontrado.");
      }
      if (!userId.Equals(video.AuthorId))
      {
        throw new NotTheOwnerException("Você não é dono deste video.");
      }

      await _repository.Delete(video);
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

    public async Task<VideosPaginatedViewModel> GetPaginatedVideos(int page, int pageSize)
    {
      List<Video>? videos = await _repository.GetAllPaginatedAsync(page, pageSize, v => v.Categoria);

      if (!videos.Any())
      {
        throw new NullReferenceException("Nenhum video foi encontrado");
      }

      List<ReadVideoDTO> videosPage = _mapper.Map<List<ReadVideoDTO>>(videos);

      var total = await _repository.GetTotalItens();

      var videosPaginated = new VideosPaginated(total, page, pageSize, videosPage);
      return videosPaginated.CreatePage();
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
