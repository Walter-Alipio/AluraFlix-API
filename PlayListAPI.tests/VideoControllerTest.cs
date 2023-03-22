using PlayListAPI.Controllers;
using PlayListAPI.Services.Interfaces;
using PlayListAPI.Data.DTOs.VideosDTOs;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace PlayListAPI.tests;

public class VideosControllerTest
{
  private Mock<IVideoServiceUserData> _moqService = new Mock<IVideoServiceUserData>();
  private IHttpContextAccessor _moqContextAccessor;
  private VideosController _controller;
  private readonly string _userIdType = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6IlVzZXIiLCJpZCI6IjdlY2IwNmQxLTE3NTUtNGYwMC04NTBjLTFkMTZjOGIyN2ViZiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJleHAiOjE2Nzg1NDE4NjN9.RGNYlVoXHCty16ye2vs9smI_E1NEDx6WKRg8O59FV0U";

  public VideosControllerTest()
  {
    _controller = new VideosController(_moqService.Object, _moqContextAccessor);
  }

  #region  POST need fix
  // [Fact]
  // public async Task TestAddVideoReturnBadRequest()
  // {
  //   // Given
  //   CreateVideoDto createVideoDto = new()
  //   {
  //     Title = "Os Gunnes",
  //     Description = "Uma aventura sem igual",
  //     Url = "www.youtube.com/wer234"
  //   };

  //   ReadVideoDTO result = new ReadVideoDTO()
  //   {
  //     Id = 1,
  //     Title = "Os Gunnes",
  //     Description = "Uma aventura sem igual",
  //     Url = "www.youtube.com/wer234"
  //   };

  //   DefaultHttpContext httpContext = new DefaultHttpContext();
  //   httpContext.Request.Headers["Authorization"] = _userIdType;
  //   var service = new Mock<IVideoServiceUserData>();
  //   IHttpContextAccessor accessor = new HttpContextAccessor();

  //   var controller = new VideosController(service.Object, accessor)
  //   {
  //     ControllerContext = new ControllerContext()
  //     {
  //       HttpContext = httpContext
  //     }
  //   };

  //   service.Setup(x => x.AddVideoAsync(createVideoDto, _userIdType)).Returns(Task.FromResult<ReadVideoDTO?>(result));
  //   // When
  //   var response = await controller.AddVideo(createVideoDto);

  //   // Then
  //   Assert.IsType<BadRequestResult>(response);
  // }

  // [Fact]
  // public async Task TestAddReturnCreatedAsync()
  // {
  //   // Given
  //   CreateVideoDto createVideoDto = new()
  //   {
  //     Title = "Os Gunnes",
  //     Description = "Uma aventura sem igual",
  //     Url = "www.youtube.com/wer234"
  //   };
  //   ReadVideoDTO result = new ReadVideoDTO()
  //   {
  //     Id = 1,
  //     Title = "Os Gunnes",
  //     Description = "Uma aventura sem igual",
  //     Url = "www.youtube.com/wer234"
  //   };
  //   DefaultHttpContext httpContext = new DefaultHttpContext();
  //   httpContext.Request.Headers["Authorization"] = _userIdType;
  //   var service = new Mock<IVideoServiceUserData>();

  //   var controller = new VideosController(service.Object, _moqContextAccessor)
  //   {
  //     ControllerContext = new ControllerContext()
  //     {
  //       HttpContext = httpContext
  //     }
  //   };

  //   service.Setup(x => x.AddVideoAsync(createVideoDto, "qwerdsf32ve")).Returns(Task.FromResult<ReadVideoDTO?>(result));

  //   // When
  //   var response = await controller.AddVideo(createVideoDto);
  //   // Then
  //   Assert.IsType<CreatedAtActionResult>(response);
  // }
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