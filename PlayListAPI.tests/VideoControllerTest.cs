using PlayListAPI.Controllers;
using PlayListAPI.Services.Interfaces;
using PlayListAPI.Data.DTOs.VideosDTOs;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Security.Claims;
using PlayListAPI.Utils;

namespace PlayListAPI.tests;

public class VideosControllerTest
{
  private Mock<IVideoServiceUserData> _moqService = new Mock<IVideoServiceUserData>();
  // private IHttpContextAccessor _moqContextAccessor;
  private Mock<ITokenExtract> _tokenServiceMock = new Mock<ITokenExtract>();
  private VideosController _controller;
  public VideosControllerTest()
  {
    _controller = new VideosController(_moqService.Object, _tokenServiceMock.Object);
    _controller.ControllerContext = new ControllerContext
    {
      HttpContext = new DefaultHttpContext
      {
        Request =
        {
            Headers =
            {
                ["Authorization"] = "Bearer [valid_token_here]"
            }
        },
        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "test") }, "Test"))
      }
    };
  }

  #region  POST need fix
  [Fact]
  public async Task AddVideo_ReturnsBadRequest_WhenVideoServiceFails()
  {
    // Arrange
    var videoDto = new CreateVideoDto();
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer valid_token_here");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns("user123");
    var expectedBadRequestResult = new BadRequestResult();

    // Simulate the video service failing to add a video
    _moqService.Setup(s => s.AddVideoAsync(It.IsAny<CreateVideoDto>(), It.IsAny<string>()))
                     .ReturnsAsync((ReadVideoDTO?)null);

    // Act
    var result = await _controller.AddVideo(videoDto);

    // Assert
    Assert.IsType<BadRequestResult>(result);
    Assert.Equal(expectedBadRequestResult.StatusCode, (result as BadRequestResult).StatusCode);
  }

  [Fact]
  public async Task AddVideo_ReturnsCreated_WhenVideoIsAddedSuccessfully()
  {
    // Arrange
    var videoDto = new CreateVideoDto();
    var tokenServiceMock = new Mock<ITokenExtract>();
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer valid_token_here");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns("user123");
    var expectedVideo = new ReadVideoDTO { Id = 1, Title = "Test Video", Description = "A video for testing purposes", Url = "https://example.com/video" };

    // Simulate adding the video successfully
    _moqService.Setup(s => s.AddVideoAsync(It.IsAny<CreateVideoDto>(), It.IsAny<string>()))
                     .ReturnsAsync(expectedVideo);

    // Act
    var result = await _controller.AddVideo(videoDto);

    // Assert
    Assert.IsType<CreatedAtActionResult>(result);
    var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
    var actualVideo = Assert.IsType<ReadVideoDTO>(createdAtResult.Value);
    Assert.Equal(expectedVideo.Id, actualVideo.Id);
    Assert.Equal(expectedVideo.Title, actualVideo.Title);
    Assert.Equal(expectedVideo.Description, actualVideo.Description);
    Assert.Equal(expectedVideo.Url, actualVideo.Url);
    Assert.Equal(StatusCodes.Status201Created, createdAtResult.StatusCode);
  }


  #endregion



  //GET
  [Fact]
  public async Task TestShowAllVideosReturnNotFoundAsync()
  {
    //Arrange
    List<ReadVideoDTO> videos = new List<ReadVideoDTO>();
    _moqService.Setup(x => x.GetVideosAsync("")).Returns(Task.FromResult<List<ReadVideoDTO>?>(videos));

    //Act
    var result = await _controller.ShowAllVideos("");
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
    var result = await _controller.ShowAllVideos("");
    // Then
    Assert.IsType<OkObjectResult>(result);
  }
  [Fact]
  public async void ShowMyVideosReturnNotFound()
  {
    ReadVideoDTO readDto = new ReadVideoDTO()
    {
      Description = "Uma aventura sem igual",
      Id = 1,
      Title = "Os Gunnes",
      Url = "www.youtube.com/wer234"
    };

    List<ReadVideoDTO> videos = new List<ReadVideoDTO>() { readDto };
    // _moqService.Setup(x => x.GetMyVideosAsync("")).Returns(Task.FromResult<List<ReadVideoDTO>?>(videos));

  }
  //PUT
  [Fact]
  public async void TestUpdateVideoReturnBadRequest()
  {
    // Given
    UpdateVideoDTO updateVideoDTO = new();
    ReadVideoDTO readVideoDTO = new();
    _moqService.Setup(x => x.UpdateVideoAsync(1, updateVideoDTO)).Throws(new ArgumentException());

    // When
    var response = await _controller.UpdateVideo(1, updateVideoDTO);
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
    // _moqService.Setup(s => s.CheckUrl(updateVideoDTO)).Returns(Result.Ok());
    var error = new NullReferenceException();
    _moqService.Setup(s => s.UpdateVideoAsync(1, updateVideoDTO)).Throws(new NullReferenceException());

    // When
    var response = await _controller.UpdateVideo(1, updateVideoDTO);
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

    // _moqService.Setup(x => x.CheckUrl(updateVideoDTO)).Returns(result);
    _moqService.Setup(x => x.UpdateVideoAsync(1, updateVideoDTO)).Returns(Task.FromResult<ReadVideoDTO?>(readVideoDTO));

    // When
    var response = await _controller.UpdateVideo(1, updateVideoDTO);
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
    var response = await _controller.DeleteVideo(1);
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
    var response = await _controller.DeleteVideo(1);
    // Then
    Assert.IsType<NoContentResult>(response);
  }

}
