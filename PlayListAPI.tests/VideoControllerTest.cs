using PlayListAPI.Services.Interfaces;
using PlayListAPI.Data.DTOs.VideosDTOs;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;

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
                ["Authorization"] = "Bearer [suppose_to_be_a_valid_token]"
            }
        },
        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "test") }, "Test"))
      }
    };
  }

  #region  POST 
  [Fact]
  public async Task AddVideo_ReturnsBadRequest_WhenVideoServiceFails()
  {
    // Arrange
    var videoDto = new CreateVideoDto();
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
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
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);
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
    _tokenServiceMock.Verify(t => t.ExtractID(headers["Authorization"]), Times.Once());

    Assert.Equal(expectedVideo.Id, actualVideo.Id);
    Assert.Equal(expectedVideo.Title, actualVideo.Title);
    Assert.Equal(expectedVideo.Description, actualVideo.Description);
    Assert.Equal(expectedVideo.Url, actualVideo.Url);
    Assert.Equal(StatusCodes.Status201Created, createdAtResult.StatusCode);
  }

  [Fact]
  public async Task AddVideo_ReturnsInternalError_WhenExtractIdFromTokenFailAsync()
  {
    // Arrange
    var videoDto = new CreateVideoDto();
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    var expectedErrorMessage = "Erro ao extrair o id";

    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Throws(new Exception(expectedErrorMessage));

    // Act
    var result = await _controller.AddVideo(videoDto) as ObjectResult;

    // Assert
    _tokenServiceMock.Verify(t => t.ExtractID(headers["Authorization"]), Times.Once());
    _tokenServiceMock.VerifyNoOtherCalls();
    Assert.NotNull(result);
    Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    Assert.Equal(expectedErrorMessage, result.Value);
  }

  #endregion

  #region GET ShowAllVideos
  [Fact]
  public async Task ShowAllVideos_ReturnNotFound_WhenVideoServiceReturnEmpity()
  {
    //Arrange
    List<ReadVideoDTO> videos = new List<ReadVideoDTO>();
    _moqService.Setup(x => x.GetVideosAsync(It.IsAny<string>())).Returns(Task.FromResult<List<ReadVideoDTO>?>(videos));

    //Act
    var result = await _controller.ShowAllVideos(It.IsAny<string>());
    //Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact]
  public async void ShowAllVideos_ReturnOkObjectResult_WithListOfVideos()
  {
    // Given
    ReadVideoDTO expectedVideo = new ReadVideoDTO()
    {
      Description = "Uma aventura sem igual",
      Id = 1,
      Title = "Os Gunnes",
      Url = "www.youtube.com/wer234"
    };

    List<ReadVideoDTO> videos = new List<ReadVideoDTO>() { expectedVideo };
    _moqService.Setup(x => x.GetVideosAsync("")).Returns(Task.FromResult<List<ReadVideoDTO>?>(videos));

    // When
    var result = await _controller.ShowAllVideos("");
    // Then
    Assert.IsType<OkObjectResult>(result);
    var objectResult = Assert.IsType<OkObjectResult>(result);
    var actualVideo = Assert.IsType<List<ReadVideoDTO>>(objectResult.Value);

    Assert.Equal(expectedVideo.Id, actualVideo[0].Id);
    Assert.Equal(expectedVideo.Title, actualVideo[0].Title);
    Assert.Equal(expectedVideo.Description, actualVideo[0].Description);
    Assert.Equal(expectedVideo.Url, actualVideo[0].Url);
    Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

  }

  #endregion

  #region Get ShowVideosPaginated
  [Fact]
  public async Task ShowVideosPaginated_ReturnNotFound_WhenPageIsZero_and_VideoServiceIsNull()
  {
    // Given
    _moqService.Setup(s => s.GetPaginatedVideos(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult<VideosPaginatedViewModel?>(null));

    // When
    var pageZero = await _controller.ShowVideosPaginated(0, 5);
    var videoServiceNull = await _controller.ShowVideosPaginated(It.IsAny<int>(), It.IsAny<int>());

    // Then
    Assert.IsType<NotFoundResult>(pageZero);
  }

  #endregion

  #region GET ShowUserVideos
  [Fact]
  public async void ShowUserVideos_ReturnOkObjectResult_WithListOfVideos()
  {
    //Arrange
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);

    List<ReadVideoDTO> expected = new List<ReadVideoDTO>() { new ReadVideoDTO()
    {
      Description = "Uma aventura sem igual",
      Id = 1,
      Title = "Os Gunnes",
      Url = "www.youtube.com/wer234"
    } };

    _moqService.Setup(x => x.GetUserVideosAsync(It.IsAny<string>())).Returns(Task.FromResult<List<ReadVideoDTO>>(expected));

    //Act
    var result = await _controller.ShowUserVideos(It.IsAny<string>());

    //Assert
    Assert.NotNull(result);
    Assert.IsType<OkObjectResult>(result);

    var objectResult = Assert.IsType<OkObjectResult>(result);
    var actualVideo = Assert.IsType<List<ReadVideoDTO>>(objectResult.Value);

    Assert.Equal(expected[0].Id, actualVideo[0].Id);
    Assert.Equal(expected[0].Title, actualVideo[0].Title);
    Assert.Equal(expected[0].Description, actualVideo[0].Description);
    Assert.Equal(expected[0].Url, actualVideo[0].Url);
    Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
  }

  [Fact]
  public async Task ShowUserVideos_ReturnNotFound_WhenVideoServiceReturnEmpity()
  {
    //Arrange
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);

    List<ReadVideoDTO> videos = new List<ReadVideoDTO>();

    _moqService.Setup(x => x.GetVideosAsync("")).Returns(Task.FromResult<List<ReadVideoDTO>?>(videos));

    //Act
    var result = await _controller.ShowUserVideos(It.IsAny<string>());
    //Assert
    Assert.IsType<NotFoundResult>(result);
  }

  [Fact]
  public async Task ShowUserVideos_ReturnsInternalError_WhenExtractIdFromTokenFailAsync()
  {
    // Arrange
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    var expectedErrorMessage = "Erro ao extrair o id";
    var authorization = headers["Authorization"];

    _tokenServiceMock.Setup(t => t.ExtractID(authorization)).Throws(new Exception(expectedErrorMessage));

    // Act
    var result = await _controller.ShowUserVideos(authorization) as ObjectResult;

    // Assert
    _tokenServiceMock.Verify(t => t.ExtractID(authorization), Times.Once());
    _tokenServiceMock.VerifyNoOtherCalls();
    Assert.NotNull(result);
    Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    Assert.Equal(expectedErrorMessage, result.Value);
  }

  #endregion

  #region  PUT
  [Fact]
  public async void UpdateVideoReturnBadRequest()
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

  #endregion

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
