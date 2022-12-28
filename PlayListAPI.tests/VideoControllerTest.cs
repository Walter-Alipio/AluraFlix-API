using PlayListAPI.Controllers;
using PlayListAPI.Services.Interfaces;
using PlayListAPI.Data.DTOs.VideosDTOs;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace PlayListAPI.tests;

public class VideosControllerTest
{
  private Mock<IVideosService> _moqService = new Mock<IVideosService>();
  private VideosController _controller;

  [Fact]
  public void TestShowAllVideosReturnNotFound()
  {
    //Arrange
    _controller = new VideosController(_moqService.Object);

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
    _controller = new VideosController(_moqService.Object);
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