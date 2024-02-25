using AutoMapper;
using FluentResults;
using PlayListAPI.Models;
using PlayListAPI.Repository;
using PlayListAPI.ViewModels;
using PlayListAPI.Profiles.CustomMapper;
using PlayListAPI.Exceptions;
using PlayListAPI.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;

namespace PlayListAPI.Services
{
  public class VideosService : IVideoServiceUserData
  {
    private readonly IMapper _mapper;
    private readonly IVideoRepository _repository;
    private readonly ICustomMapVideo _customMapper;

    public VideosService(IMapper mapper, IVideoRepository repository, ICustomMapVideo customMapper)
    {
      _mapper = mapper;
      _repository = repository;
      _customMapper = customMapper;
    }

    public async Task<ReadVideoDTO> AddVideoAsync(CreateVideoDto videoDto, string userId)
    {
      Result resultado = CheckUrlPattern(videoDto);

      if (resultado.IsFailed)
      {
        throw new ArgumentException("Formato de url inválido");
      }
      if (videoDto.CategoriaId == 0) videoDto.CategoriaId = 1;

      Video video = _mapper.Map<Video>(videoDto);

      video.AuthorId = userId;
      video.CreatedAt = DateTime.Now;
      video.ModifyAt = DateTime.Now;

      await _repository.AddAsync(video);

      return _mapper.Map<ReadVideoDTO>(video);
    }


    public async Task<ReadVideoDTO> GetVideoByIdAsync(int id)
    {
      Video? video = await _repository.GetByIdAsync(id, v => v.Categoria!) ?? throw new NullReferenceException("Video não encontrado.");

      return _mapper.Map<ReadVideoDTO>(video);
    }


    public async Task<List<ReadVideoDTO>> GetVideosAsync(string? videoTitle)
    {
      List<Video>? videos = await _repository.GetAll(v => v.Categoria!);
      if (videos is null || !videos.Any())
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
      Video? video = await _repository.GetByIdAsync(id) ?? throw new NullReferenceException("Video não encontrado.");

      if (!userId.Equals(video.AuthorId))
      {
        throw new NotTheOwnerException("Você não é dono deste video.");
      }

      if (!string.IsNullOrEmpty(videoDTO.Url))
      {
        var result = CheckUrlPattern(videoDTO);

        if (result.IsFailed)
        {
          throw new ArgumentException(result.Errors.First().ToString());
        }
      }

      _customMapper.MapUpdateDtoToVideo(videoDTO, video);

      video.ModifyAt = DateTime.Now;

      await _repository.UpdateAsync(video);

      return _mapper.Map<ReadVideoDTO>(video);
    }

    public async Task DeleteVideoAsync(int id, string userId)
    {
      Video? video = await _repository.GetByIdAsync(id) ?? throw new NullReferenceException("Video não encontrado.");

      if (!userId.Equals(video.AuthorId))
      {
        throw new NotTheOwnerException("Você não permissão para deletar este video.");
      }

      await _repository.Delete(video);
    }

    public async Task<List<ReadVideoDTO>> GetUserVideosAsync(string userId)
    {
      List<Video> videos = await _repository.GetAllUserVideos(userId);
      if (videos is null || !videos.Any())
      {
        return new List<ReadVideoDTO>();
      }

      return _mapper.Map<List<ReadVideoDTO>>(videos);
    }

    public async Task<VideosPaginatedViewModel> GetPaginatedVideos(int page, int pageSize, string? videoTitle)
    {
      List<Video>? videos = await _repository.GetAllPaginatedAsync(page, pageSize, v => v.Categoria!);

      if (videos is null || !videos.Any())
      {
        throw new NullReferenceException("Nenhum video foi encontrado");
      }

      if (!string.IsNullOrEmpty(videoTitle))
      {
        try
        {
          videos = videos.Where(v => v.Title.Contains(videoTitle)).ToList();
        }
        catch (NullReferenceException)
        {
          throw new NullReferenceException("Nenhum video foi encontrado");
        }
      }

      List<ReadVideoDTO> videosPage = _mapper.Map<List<ReadVideoDTO>>(videos);

      int total = await _repository.GetTotalItens();

      var videosPaginated = new VideosPaginated(total, page, pageSize, videosPage);
      return videosPaginated.CreatePage();
    }

    private static Result CheckUrlPattern(VideoDto videoDto)
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
