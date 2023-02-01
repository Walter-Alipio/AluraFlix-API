using AutoMapper;
using FluentResults;
using Moq;
using PlayListAPI.Data.DAOs.Interfaces;
using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;
using PlayListAPI.Services;
using PlayListAPI.Services.Interfaces;

namespace PlayListAPI.tests;

public class VideoServiceTest
{
  private Mock<IVideoDAO> _MockDao = new Mock<IVideoDAO>();

  private IVideosService _service;
  public VideoServiceTest()
  {
    //To use the real map function
    var profile = new VideoProfile();
    var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
    IMapper mapper = new Mapper(configuration);

    _service = new VideosService(mapper, _MockDao.Object);
  }
  [Fact]
  public async void TestGetVideosReturnNullAsync()
  {
    // Given
    var videos = new List<Video>();
    _MockDao.Setup(d => d.GetVideos()).Returns(Task.FromResult(videos));
    var expct = new List<ReadVideoDTO>();
    // When
    var result = await _service.GetVideosAsync("");

    // Then
    Assert.Equal(expct, result);
  }

  [Fact]
  public async Task TestGetVideosWithParamsReturnEmptyListAsync()
  {
    // Given
    var title = "Tentei fazer uma API e olha no que deu";

    var videos = new List<Video>()
    {
       new Video() {Title = "Como se tornar desenvolvedor em 3 passos"},
       new Video() {Title = "Front-end vs Back-end"},
       new Video() {Title = "Bolha Tec"},
    };
    var expct = new List<ReadVideoDTO>();

    _MockDao.Setup(d => d.GetVideos()).Returns(Task.FromResult(videos));
    // When
    var result = await _service.GetVideosAsync(title);
    // Then
    Assert.Empty(result);
  }

  [Fact]
  public async void TestGeVideosReturnListAsync()
  {
    // Given
    var title = "Tentei fazer uma API e olha no que deu";

    var videos = new List<Video>()
    {
       new Video() {Title = "Como se tornar desenvolvedor em 3 passos"},
       new Video() {Title = title},
       new Video() {Title = "Front-end vs Back-end"},
       new Video() {Title = "Bolha Tec"},
    };

    _MockDao.Setup(d => d.GetVideos()).Returns(Task.FromResult(videos));
    // When
    var resultNoParams = await _service.GetVideosAsync("");
    var resultWithParams = await _service.GetVideosAsync(title);
    // Then
    Assert.IsType<List<ReadVideoDTO>>(resultNoParams);
    Assert.NotEmpty(resultWithParams);
  }

  [Fact]
  public async void TestGetVideoByIdReturnNullAsync()
  {
    // Given
    int id = 0;

    _MockDao.Setup(d => d.GetByIdAsync(id)).Returns(Task.FromResult<Video?>(null));

    // When
    var result = await _service.GetVideoByIdAsync(0);
    // Then
    Assert.Null(result);

  }

  [Fact]
  public async void TestUpdateVideoAsyncReturnNullAsync()
  {
    // Given
    int id = 0;
    var videoDTO = new UpdateVideoDTO();

    _MockDao.Setup(d => d.GetByIdAsync(id)).Returns(Task.FromResult<Video?>(null));

    // When
    var result = await _service.UpdateVideoAsync(id, videoDTO);
    // Then
    Assert.Null(result);
  }

  [Fact]
  public async Task TestUpdateVideoAsyncReturnReadVideoAsync()
  {
    // Given

    int id = 0;
    var videoDTO = new UpdateVideoDTO();
    var video = new Video();

    _MockDao.Setup(d => d.GetByIdAsync(id)).Returns(Task.FromResult<Video?>(video));

    // When
    var result = await _service.UpdateVideoAsync(id, videoDTO);
    // Then
    _MockDao.Verify(d => d.UpdateAsync(video), Times.Once);
    Assert.IsType<ReadVideoDTO>(result);
  }

  [Fact]
  public async void TestDeleteVideoAsyncReturnResultFail()
  {
    // Given
    int id = 0;
    var expct = new Result();

    _MockDao.Setup(d => d.GetByIdAsync(id)).Returns(Task.FromResult<Video?>(null));

    // When
    var result = await _service.DeleteVideoAsync(id);

    // Then
    Assert.True(result.IsFailed);
  }

  [Fact]
  public async void TestDeleteVideoAsyncReturnResultSuccess()
  {
    // Given
    int id = 0;
    var video = new Video();
    var expct = new Result();


    _MockDao.Setup(d => d.GetByIdAsync(id)).Returns(Task.FromResult<Video?>(video));

    // When
    var result = await _service.DeleteVideoAsync(id);

    // Then
    _MockDao.Verify(d => d.Delete(video), Times.Once);
    Assert.True(result.IsSuccess);
  }

  [Fact]
  public async void TestAddVideoAsyncReturnFail()
  {
    // Given
    CreateVideoDto videoDto = new()
    {
      CategoriaId = 1,
      Title = "Aprenda a base",
      Url = "www.youtube.com/1234",
      Description = "Quais os passos para iniciar na programação"
    };

    // When
    var result = await _service.AddVideoAsync(videoDto);
    // Then
    Assert.Null(result);
  }

  [Fact]
  public async void TestAddVideoAsyncReturnSuccess()
  {
    // Given
    CreateVideoDto videoDto = new()
    {
      CategoriaId = 1,
      Title = "Aprenda a base",
      Url = "https://www.youtube.com/watch?v=BTENKdRVS2U",
      Description = "O MÍNIMO QUE VOCÊ PRECISA SABER ANTES DE PROGRAMAR!"
    };

    // When
    var result = await _service.AddVideoAsync(videoDto);
    // Then

    Assert.IsType<ReadVideoDTO>(result);
  }

}