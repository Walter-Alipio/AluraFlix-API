using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;
using PlayListAPI.Services.Interfaces;
using AutoMapper;
using FluentResults;
using PlayListAPI.Repository;

namespace PlayListAPI.Services
{
    public class VideosService : IVideosService
    {
        private IMapper _mapper;
        private IVideoRepository _dao;
        public VideosService(IMapper mapper, IVideoRepository dao)
        {
            _mapper = mapper;
            _dao = dao;
        }



        public async Task<ReadVideoDTO?> AddVideoAsync(CreateVideoDto videoDto)
        {
            Result resultado = CheckUrlPattern(videoDto);

            if (resultado.IsFailed)
            {
                return null;
            }

            Video video = _mapper.Map<Video>(videoDto);

            await _dao.AddAsync(video);

            return _mapper.Map<ReadVideoDTO>(video);
        }


        public async Task<ReadVideoDTO?> GetVideoByIdAsync(int id)
        {
            Video? video = await _dao.GetByIdAsync(id, v => v.Categoria);
            if (video == null)
            {
                return null;
            }
            return _mapper.Map<ReadVideoDTO>(video);
        }


        public async Task<List<ReadVideoDTO>?> GetVideosAsync(string? videoTitle)
        {
            List<Video> videos = await _dao.GetAll(v => v.Categoria);
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

            Video? video = await _dao.GetByIdAsync(id, v => v.Categoria)!;
            if (video == null) return null;

            _mapper.Map(videoDTO, video);

            await _dao.UpdateAsync(video);

            return _mapper.Map<ReadVideoDTO>(video);
        }


        public async Task<Result> DeleteVideoAsync(int id)
        {
            Video? video = await _dao.GetByIdAsync(id);
            if (video is null)
            {
                return Result.Fail("Video não encontrado.");
            }

            await _dao.Delete(video);

            return Result.Ok();
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

        public Result CheckUrl(UpdateVideoDTO videoDTO)
        {
            return CheckUrlPattern(videoDTO);
        }

    }
}
