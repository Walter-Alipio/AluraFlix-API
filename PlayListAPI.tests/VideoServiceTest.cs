using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Moq;
using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Exceptions;
using PlayListAPI.Models;
using PlayListAPI.Repository;
using PlayListAPI.Services;
using PlayListAPI.Services.Interfaces;
using PlayListAPI.ViewModels.CustomMapper;
using PlayListAPI.ViewModels.Profiles;

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

    var _customMapper = new CustomMapVideo();
    _service = new VideosService(mapper, _MockRepository.Object, _customMapper);
  }

  [Fact]
  public async void GetVideosAsync_ReturnsEmptyList_WhenRepositoryReturnsNull()
  {
    // Given
    _MockRepository.Setup(d => d.GetAll(null)).Returns(Task.FromResult<List<Video>?>(null));
    // When
    var result = await _service.GetVideosAsync(null);

    // Then
    Assert.Empty(result);
  }

  [Fact]
  public async Task GetVideos_ReturnsEmptyList_WhenVideoNameIsNotFound()
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

    _MockRepository.Setup(d => d.GetAll(null)).Returns(Task.FromResult<List<Video>?>(videos));
    // When
    var result = await _service.GetVideosAsync(title);
    // Then
    Assert.NotNull(result);
    Assert.Empty(result);
    Assert.IsType<List<ReadVideoDTO>>(result);
  }

  [Fact]
  public async void GeVideos_ReturnsReadVideoDTOList_WhenGetAllIsSuccessful()
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

    _MockRepository.Setup(d => d.GetAll(v => v.Categoria!)).Returns(Task.FromResult<List<Video>?>(videos));
    // When
    var resultNoParams = await _service.GetVideosAsync("");
    var resultWithParams = await _service.GetVideosAsync(title);
    // Then
    Assert.IsType<List<ReadVideoDTO>>(resultNoParams);
    Assert.Equal(videos.Count(), resultNoParams.Count());
    Assert.NotEmpty(resultWithParams);
    Assert.Equal(title, resultWithParams[0].Title);
  }

  [Fact]
  public async void GetVideoById_ThrowsNullReferenceException_WhenVideoIsNotFound()
  {
    // Given
    var expectedMessage = "Video não encontrado.";

    _MockRepository.Setup(d => d.GetByIdAsync(It.IsAny<int>(), null)).Returns(Task.FromResult<Video?>(null));

    // When
    var result = await Assert.ThrowsAsync<NullReferenceException>(() => _service.GetVideoByIdAsync(It.IsAny<int>()));
    // Then
    Assert.NotNull(result);
    Assert.Equal(expectedMessage, result.Message);

  }

  [Fact]
  public async Task GetUserVideo_ReturnsEmptyList_WhenThereIsNoUserVideos()
  {
    // Given
    List<Video> videos = new List<Video>();
    _MockRepository.Setup(d => d.GetAllUserVideos(It.IsAny<string>())).Returns(Task.FromResult(videos));

    // When
    var result = await _service.GetUserVideosAsync(It.IsAny<string>());

    // Then
    Assert.NotNull(result);
    Assert.Empty(result);
    Assert.IsType<List<ReadVideoDTO>>(result);
  }

  [Fact]
  public async void GetUserVideo_ReturnsReadVideoDTOList_WhenGetUserVideosIsSuccessful()
  {
    // Given
    var userId = "gh234gh1234g";

    var videos = new List<Video>()
        {
             new Video() {Title = "Como se tornar desenvolvedor em 3 passos" , AuthorId = userId},
             new Video() {Title = "Front-end vs Back-end", AuthorId = userId},
             new Video() {Title = "Bolha Tec" , AuthorId = userId},
        };

    _MockRepository.Setup(d => d.GetAllUserVideos(userId)).Returns(Task.FromResult(videos));
    // When
    var result = await _service.GetUserVideosAsync(userId);
    // Then
    Assert.NotEmpty(result);
    Assert.IsType<List<ReadVideoDTO>>(result);
  }

  [Fact]
  public async void UpdateVideoAsync_ThrowsNullReferenceException_WhenVideoNotFound()
  {
    // Given
    var expectedMessage = "Video não encontrado.";
    _MockRepository.Setup(d => d.GetByIdAsync(It.IsAny<int>(), null)).Returns(Task.FromResult<Video?>(null));

    // When
    // Then
    var restul = await Assert.ThrowsAsync<NullReferenceException>(() => _service.UpdateVideoAsync(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>(), It.IsAny<string>()));
    Assert.NotNull(restul);
    Assert.Equal(expectedMessage, restul.Message);
  }

  [Fact]
  public async Task UpdateVideoAsync_ReturnReadVideo_WhenUpdateIsSuccessful()
  {
    // Given
    var userId = "sorte";
    var videoDTO = new UpdateVideoDTO();
    Video expected = new Video()
    {
      Id = 123,
      Title = "teste",
      AuthorId = userId,
      Description = "teste automatizado do delete",
      CategoriaId = 1,
    };

    _MockRepository.Setup(d => d.GetByIdAsync(It.IsAny<int>(), null)).Returns(Task.FromResult<Video?>(expected));
    _MockRepository.Setup(d => d.UpdateAsync(It.IsAny<Video>())).Returns(Task.CompletedTask);

    // When
    var result = await _service.UpdateVideoAsync(It.IsAny<int>(), videoDTO, userId);

    // Then
    _MockRepository.Verify(d => d.UpdateAsync(expected), Times.Once);
    Assert.NotNull(result);
    Assert.IsType<ReadVideoDTO>(result);
    Assert.Equal(expected.Id, result.Id);
    Assert.Equal(expected.Url, result.Url);
    Assert.Equal(expected.Categoria, result.Categoria);
    Assert.Equal(expected.Description, result.Description);
  }

  [Fact]
  public async void DeleteVideoAsync_ThrowsNullReferenceException_WhenVideoNotFound()
  {
    // Given
    var expectedMessage = "Video não encontrado.";
    _MockRepository.Setup(d => d.GetByIdAsync(It.IsAny<int>(), null)).Returns(Task.FromResult<Video?>(null));

    // When
    var result = await Assert.ThrowsAsync<NullReferenceException>(() => _service.DeleteVideoAsync(It.IsAny<int>(), It.IsAny<string>()));
    // Then
    Assert.NotNull(result);
    Assert.Equal(expectedMessage, result.Message);
  }

  [Fact]
  public async void DeleteVideoAsync_ThrowsNotTheOwnerException_WhenUserIsNotTheVideoOwner()
  {
    var expectedMessage = "Você não permissão para deletar este video.";
    var userId = "DFWER@#$";
    var video = new Video()
    {
      Title = "Como se tornar desenvolvedor em 3 passos",
      AuthorId = "Er@#4",
      Categoria = new Categoria(),
      CategoriaId = 1,
      Id = 1
    };

    // Given
    _MockRepository.Setup(d => d.GetByIdAsync(It.IsAny<int>(), null)).Returns(Task.FromResult<Video?>(video));

    // When
    // Then
    var result = await Assert.ThrowsAsync<NotTheOwnerException>(() => _service.DeleteVideoAsync(It.IsAny<int>(), userId));
    Assert.NotNull(result);
    Assert.IsType<NotTheOwnerException>(result);
    Assert.Equal(expectedMessage, result.Message);
  }

  [Fact]
  public async void DeleteVideoAsync_CallsRepositoryDelete_WhenDeleteIsSuccessful()
  {
    // Given
    var userId = "sorte";

    var video = new Video()
    {
      Id = 123,
      Title = "teste",
      AuthorId = userId,
      Description = "teste automatizado do delete",
      CategoriaId = 1,
    };

    _MockRepository.Setup(d => d.GetByIdAsync(It.IsAny<int>(), null)).Returns(Task.FromResult<Video?>(video));

    // When
    await _service.DeleteVideoAsync(It.IsAny<int>(), userId);

    // Then
    _MockRepository.Verify(d => d.Delete(video), Times.Once);
  }

  [Fact]
  public async void AddVideoAsync_ThrowsArgumentException_WhenUrlFormatInvalid()
  {
    // Given
    var expectedMessage = "Formato de url inválido";
    CreateVideoDto videoDto = new()
    {
      CategoriaId = 1,
      Title = "Aprenda a base",
      Url = "www.youtube.com/1234",
      Description = "Quais os passos para iniciar na programação"
    };
    string userId = "!@#FAERG#$@!@#CR$V%T%TY$%Y";

    // When
    var result = await Assert.ThrowsAsync<ArgumentException>(() => _service.AddVideoAsync(videoDto, userId));
    // Then
    Assert.NotNull(result);
    Assert.IsType<ArgumentException>(result);
    Assert.Equal(expectedMessage, result.Message);
  }

  [Fact]
  public async void AddVideoAsync_ReturnReadVideoDTO_WhenAddIsSuccessful()
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
    var result = await _service.AddVideoAsync(videoDto, It.IsAny<string>());
    // Then

    Assert.NotNull(result);
    Assert.Equal(videoDto.Title, result.Title);
    Assert.Equal(videoDto.Description, result.Description);
    Assert.Equal(videoDto.Url, result.Url);
  }

}