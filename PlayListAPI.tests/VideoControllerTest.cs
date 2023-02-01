using PlayListAPI.Controllers;
using PlayListAPI.Services.Interfaces;
using PlayListAPI.Data.DTOs.VideosDTOs;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentResults;

namespace PlayListAPI.tests;

public class VideosControllerTest
{
  private Mock<IVideosService> _moqService = new Mock<IVideosService>();
  private VideosController _controller;

  public VideosControllerTest()
  {
    _controller = new VideosController(_moqService.Object);
  }

  //POST
  [Fact]
  public async void TestAddVideoReturnBadRequest()
  {
    // Given
    CreateVideoDto createVideoDto = new();
    _moqService.Setup(x => x.AddVideoAsync(createVideoDto)).Returns(Task.FromResult<ReadVideoDTO?>(null));
    // When
    var response = await _controller.addVideo(createVideoDto);
    // Then
    Assert.IsType<BadRequestObjectResult>(response);
  }
  [Fact]
  public async Task TestAddReturnCreatedAsync()
  {
    // Given
    CreateVideoDto createVideoDto = new()
    {
      Title = "Os Gunnes",
      Description = "Uma aventura sem igual",
      Url = "www.youtube.com/wer234"
    };
    var result = new ReadVideoDTO();
    _moqService.Setup(x => x.AddVideoAsync(createVideoDto)).Returns(Task.FromResult<ReadVideoDTO?>(result));

    // When
    var response = await _controller.addVideo(createVideoDto);
    // Then
    Assert.IsType<CreatedAtActionResult>(response);
  }

  //GET
  [Fact]
  public async Task TestShowAllVideosReturnNotFoundAsync()
  {
    //Arrange
    List<ReadVideoDTO> videos = new List<ReadVideoDTO>();
    _moqService.Setup(x => x.GetVideosAsync("")).Returns(Task.FromResult<List<ReadVideoDTO>?>(videos));

    //Act
    var result = await _controller.showAllVideos("");
    //Assert
    Assert.IsType<NotFoundResult>(result);
  }
  [Fact]
  public async void TestShowAllVideosReturnOk()
  {
    // Given
    ReadVideoDTO readDto = new ReadVideoDTO()
    {
      Description = "Uma aventura sem igual",
      Id = 1,
      Title = "Os Gunnes",
      Url = "www.youtube.com/wer234"
    };

    List<ReadVideoDTO> videos = new List<ReadVideoDTO>() { readDto };
    _moqService.Setup(x => x.GetVideosAsync("")).Returns(Task.FromResult<List<ReadVideoDTO>?>(videos));

    // When
    var result = await _controller.showAllVideos("");
    // Then
    Assert.IsType<OkObjectResult>(result);
  }

  //PUT
  [Fact]
  public async void TestUpdateVideoReturnBadRequest()
  {
    // Given
    UpdateVideoDTO updateVideoDTO = new();
    ReadVideoDTO readVideoDTO = new();
    var result = Result.Fail("");
    _moqService.Setup(x => x.CheckUrl(updateVideoDTO)).Returns(result);

    // When
    var response = await _controller.updateVideo(1, updateVideoDTO);
    // Then
    Assert.IsType<BadRequestObjectResult>(response);
  }
  [Fact]
  public async void TestUpdateVideoReturnNotFound()
  {
    // Given
    UpdateVideoDTO updateVideoDTO = new();
    ReadVideoDTO? readVideoDTO = new();
    readVideoDTO = null;
    _moqService.Setup(s => s.CheckUrl(updateVideoDTO)).Returns(Result.Ok());
    _moqService.Setup(s => s.UpdateVideoAsync(1, updateVideoDTO)).Returns(Task.FromResult<ReadVideoDTO?>(null));

    // When
    var response = await _controller.updateVideo(1, updateVideoDTO);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }
  [Fact]
  public async void TestUpdateVideoReturnCreated()
  {
    // Given
    UpdateVideoDTO updateVideoDTO = new();
    ReadVideoDTO readVideoDTO = new();

    var result = Result.Ok();

    _moqService.Setup(x => x.CheckUrl(updateVideoDTO)).Returns(result);
    _moqService.Setup(x => x.UpdateVideoAsync(1, updateVideoDTO)).Returns(Task.FromResult<ReadVideoDTO?>(readVideoDTO));

    // When
    var response = await _controller.updateVideo(1, updateVideoDTO);
    // Then
    Assert.IsType<CreatedAtActionResult>(response);
  }

  //DELETE
  [Fact]
  public async void TestDeleteVideoReturnNotFound()
  {
    // Given
    Result result = Result.Fail("Não encontrado");
    _moqService.Setup(x => x.DeleteVideoAsync(1)).Returns(Task.FromResult(result));
    // When
    var response = await _controller.deleteVideo(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }
  [Fact]
  public async void TestDeleteVideoReturnNoContent()
  {
    // Given
    Result result = Result.Ok();
    _moqService.Setup(x => x.DeleteVideoAsync(1)).Returns(Task.FromResult(result));
    // When
    var response = await _controller.deleteVideo(1);
    // Then
    Assert.IsType<NoContentResult>(response);
  }
}