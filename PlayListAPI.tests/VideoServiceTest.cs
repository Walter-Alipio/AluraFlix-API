using AutoMapper;
using FluentResults;
using Moq;
using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;
using PlayListAPI.Repository;
using PlayListAPI.Services;
using PlayListAPI.Services.Interfaces;

namespace PlayListAPI.tests;

public class VideoServiceTest
{
  private Mock<IVideoRepository> _MockRepository = new Mock<IVideoRepository>();

  private IVideoServiceUserData _service;
  public VideoServiceTest()
  {
    //To use the real map function
    var profile = new VideoProfile();
    var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
    IMapper mapper = new Mapper(configuration);

    _service = new VideosService(mapper, _MockRepository.Object);
  }
  [Fact]
  public async void TestGetVideosReturnNullAsync()
  {
    // Given
    var videos = new List<Video>();
    _MockRepository.Setup(d => d.GetAll(v => v.Categoria)).Returns(Task.FromResult(videos));
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

    _MockRepository.Setup(d => d.GetAll(v => v.Categoria)).Returns(Task.FromResult(videos));
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

    _MockRepository.Setup(d => d.GetAll(v => v.Categoria)).Returns(Task.FromResult(videos));
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

    _MockRepository.Setup(d => d.GetByIdAsync(id, v => v.Categoria)).Returns(Task.FromResult<Video?>(null));

    // When
    var result = await _service.GetVideoByIdAsync(0);
    // Then
    Assert.Null(result);

  }

  [Fact]
  public async Task TestGetUserVideoReturnsEmptyAsync()
  {
    // Given
    var userId = "gh234gh1234g";

    List<Video> videos = new List<Video>();
    _MockRepository.Setup(d => d.GetAllUserVideos(userId)).Returns(Task.FromResult(videos));
    // When
    var result = await _service.GetUserVideosAsync(userId);
    // Then
    Assert.Empty(result);
  }
  [Fact]
  public async void TestGetUserVideoReturnsListAsync()
  {
    // Given
    var userId = "gh234gh1234g";

    var videos = new List<Video>()
        {
             new Video() {Title = "Como se tornar desenvolvedor em 3 passos" , AuthorId = "gh234gh1234g"},
             new Video() {Title = "Front-end vs Back-end", AuthorId = "gh234gh1234g"},
             new Video() {Title = "Bolha Tec" , AuthorId = "gh234gh1234g"},
        };

    _MockRepository.Setup(d => d.GetAllUserVideos(userId)).Returns(Task.FromResult(videos));
    // When
    var result = await _service.GetUserVideosAsync(userId);
    // Then
    Assert.IsType<List<ReadVideoDTO>>(result);
  }

  [Fact]
  public async void TestUpdateVideoAsyncReturnNullAsync()
  {
    // Given
    int id = 0;
    var videoDTO = new UpdateVideoDTO();

    _MockRepository.Setup(d => d.GetByIdAsync(id, v => v.Categoria)).Returns(Task.FromResult<Video?>(null));

    // When

    // Then
    await Assert.ThrowsAsync<NullReferenceException>(async () => await _service.UpdateVideoAsync(id, videoDTO, It.IsAny<string>()));
  }

  [Fact]
  public async Task TestUpdateVideoAsyncReturnReadVideoAsync()
  {
    // Given

    int id = 0;
    var videoDTO = new UpdateVideoDTO();
    var video = new Video();

    _MockRepository.Setup(d => d.GetByIdAsync(id, v => v.Categoria)).Returns(Task.FromResult<Video?>(video));

    // When
    var result = await _service.UpdateVideoAsync(id, videoDTO, It.IsAny<string>());
    // Then
    _MockRepository.Verify(d => d.UpdateAsync(video), Times.Once);
    Assert.IsType<ReadVideoDTO>(result);
  }

  [Fact]
  public async void TestDeleteVideoAsyncReturnResultFail()
  {
    // Given
    int id = 0;
    var expct = new Result();

    _MockRepository.Setup(d => d.GetByIdAsync(id, v => v.Categoria)).Returns(Task.FromResult<Video?>(null));

    // When
    // var result = await _service.DeleteVideoAsync(id, It.IsAny<string>());

    // Then
    // Assert.True(result.IsFailed);
  }

  [Fact]
  public async void TestDeleteVideoAsyncReturnResultSuccess()
  {
    // Given
    int id = 123;
    var video = new Video()
    {
      Id = 123,
      Title = "teste",
      Description = "teste automatizado do delete",
      CategoriaId = 1,
    };
    var expct = new Result();


    _MockRepository.Setup(d => d.GetByIdAsync(id, null)).Returns(Task.FromResult<Video?>(video));

    // When
    // var result = await _service.DeleteVideoAsync(id, It.IsAny<string>());

    // Then
    _MockRepository.Verify(d => d.Delete(video), Times.Once);
    // Assert.True(result.IsSuccess);
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
    string userId = "!@#FAERG#$@!@#CR$V%T%TY$%Y";

    // When
    var result = await _service.AddVideoAsync(videoDto, userId);
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
    string userId = "!@#FAERG#$@!@#CR$V%T%TY$%Y";

    // When
    var result = await _service.AddVideoAsync(videoDto, userId);
    // Then

    Assert.IsType<ReadVideoDTO>(result);
  }

}