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

  [Fact]
  public void TestAddVideoReturnBadRequest()
  {
    // Given
    CreateVideoDto createVideoDto = new();
    var result = Result.Fail("");
    _moqService.Setup(x => x.addVideo(createVideoDto)).Returns(result);
    // When
    var response = _controller.addVideo(createVideoDto);
    // Then
    Assert.IsType<BadRequestObjectResult>(response);
  }
  [Fact]
  public void TestAddReturnCreated()
  {
    // Given
    CreateVideoDto createVideoDto = new()
    {
      Title = "Os Gunnes",
      Description = "Uma aventura sem igual",
      Url = "www.youtube.com/wer234"
    };
    var result = Result.Ok();
    _moqService.Setup(x => x.addVideo(createVideoDto)).Returns(result);

    // When
    var response = _controller.addVideo(createVideoDto);
    // Then
    Assert.IsType<CreatedResult>(response);
  }

  [Fact]
  public void TestShowAllVideosReturnNotFound()
  {
    //Arrange
    List<ReadVideoDTO> videos = new List<ReadVideoDTO>();
    _moqService.Setup(x => x.ShowAllVideos("")).Returns(videos);

    //Act
    var result = _controller.showAllVideos("");
    //Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact]
  public void TestShowAllVideosReturnOk()
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
    _moqService.Setup(x => x.ShowAllVideos("")).Returns(videos);

    // When
    var result = _controller.showAllVideos("");
    // Then
    Assert.IsType<OkObjectResult>(result);
  }


}