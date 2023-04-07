using PlayListAPI.Services.Interfaces;
using PlayListAPI.Data.DTOs.VideosDTOs;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using PlayListAPI.Exceptions;

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

  #region POST 
  [Fact]
  public async Task AddVideo_ReturnsBadRequest_WhenVideoThrowArgumentException()
  {
    // Arrange
    var videoDto = new CreateVideoDto();
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns("user123");

    var errorMessage = "Formato de url inválido";
    // Simulate the video service failing to add a video
    _moqService.Setup(s => s.AddVideoAsync(It.IsAny<CreateVideoDto>(), It.IsAny<string>()))
                     .Throws(new ArgumentException(errorMessage));

    // Act
    var response = await _controller.AddVideo(videoDto) as ObjectResult;

    // Assert
    Assert.NotNull(response);
    Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
    Assert.Equal(errorMessage, response.Value);
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

    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Throws(new ErrorToGetUserIdException(expectedErrorMessage));

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

  #region GET ShowVideoById

  [Fact]
  public async Task ShowVideoById_ReturnsNotFound_WhenServiceThrowsNullException()
  {
    // Given
    var errorMessage = "Video não encontrado";
    _moqService.Setup(s => s.GetVideoByIdAsync(It.IsAny<int>())).Throws(new NullReferenceException(errorMessage));

    // When
    var response = await _controller.ShowVideoById(It.IsAny<int>()) as ObjectResult;

    // Then
    Assert.NotNull(response);
  }

  [Fact]
  public async void ShowVideoById_ReturnOkObjectResult_WithVideo()
  {
    //Arrange

    var expected = new ReadVideoDTO()
    {
      Description = "Uma aventura sem igual",
      Id = 1,
      Title = "Os Gunnes",
      Url = "www.youtube.com/wer234"
    };

    _moqService.Setup(x => x.GetVideoByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<ReadVideoDTO>(expected));

    //Act
    var result = await _controller.ShowVideoById(It.IsAny<int>());

    //Assert
    Assert.NotNull(result);
    Assert.IsType<OkObjectResult>(result);

    var objectResult = Assert.IsType<OkObjectResult>(result);
    var actualVideo = Assert.IsType<ReadVideoDTO>(objectResult.Value);

    Assert.Equal(expected.Id, actualVideo.Id);
    Assert.Equal(expected.Title, actualVideo.Title);
    Assert.Equal(expected.Description, actualVideo.Description);
    Assert.Equal(expected.Url, actualVideo.Url);
    Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
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
    var errorMessage = "Nenhum video encontrado";
    _moqService.Setup(s => s.GetPaginatedVideos(It.IsAny<int>(), It.IsAny<int>())).Throws(new NullReferenceException(errorMessage));

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

    _tokenServiceMock.Setup(t => t.ExtractID(authorization)).Throws(new ErrorToGetUserIdException(expectedErrorMessage));

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

  #region PUT
  [Fact]
  public async void UpdateVideo_ReturnBadRequest_WhenVideoServiceThrowArgumentException()
  {
    // Given
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);

    _moqService.Setup(x => x.UpdateVideoAsync(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>(), It.IsAny<string>())).Throws(new ArgumentException());
    // When
    var responseArgumentE = await _controller.UpdateVideo(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>());

    // Then
    Assert.IsType<BadRequestObjectResult>(responseArgumentE);
  }

  [Fact]
  public async void UpdateVideo_ReturnNotFound_WhenVideoServiceIsNull_OrThrowNullException()
  {
    // Given
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);

    var errorMessage = "Video não encontrado";
    _moqService.Setup(x => x.UpdateVideoAsync(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>(), It.IsAny<string>()))
        .Throws(new NullReferenceException(errorMessage));

    // When
    var response = await _controller.UpdateVideo(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>()) as ObjectResult;

    // Then
    Assert.NotNull(response);
    Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    Assert.Equal(errorMessage, response.Value);
  }

  [Fact]
  public async Task UpdateVideo_ReturnCreated_WhenUpdateVideoSuccessfully()
  {
    // Given
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);

    ReadVideoDTO expected = new()
    {
      Id = 1,
      Categoria = new Models.Categoria() { Id = 1, Cor = "#ffffff", Title = "LIVRE" },
      Description = "Teste do update video",
      Title = "Teste Unitário",
      Url = "https://www.youtoube.com/12341ewr1"
    };

    _moqService.Setup(x => x.UpdateVideoAsync(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>(), It.IsAny<string>())).Returns(Task.FromResult<ReadVideoDTO?>(expected));

    // When
    var response = await _controller.UpdateVideo(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>());
    // Then
    Assert.IsType<CreatedAtActionResult>(response);

    var result = Assert.IsType<CreatedAtActionResult>(response);
    var actualVideo = Assert.IsType<ReadVideoDTO>(result.Value);

    Assert.Equal(expected.Id, actualVideo.Id);
    Assert.Equal(expected.Title, actualVideo.Title);
    Assert.Equal(expected.Description, actualVideo.Description);
    Assert.Equal(expected.Url, actualVideo.Url);
    Assert.Equal(expected.Categoria, actualVideo.Categoria);
    Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
  }

  [Fact]
  public async Task UpdateVideo_ReturnsInternalError_WhenExtractIdFromTokenFailAsync()
  {
    // Arrange
    var videoDto = new CreateVideoDto();
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    var expectedErrorMessage = "Erro ao extrair o id";

    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Throws(new ErrorToGetUserIdException(expectedErrorMessage));

    // Act
    var result = await _controller.UpdateVideo(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>()) as ObjectResult;

    // Assert
    _tokenServiceMock.Verify(t => t.ExtractID(headers["Authorization"]), Times.Once());
    _tokenServiceMock.VerifyNoOtherCalls();
    Assert.NotNull(result);
    Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    Assert.Equal(expectedErrorMessage, result.Value);
  }

  [Fact]
  public async void UpdateVideo_ReturnMethodNotAllowed_WhenVideoServiceThrowNotTheVideoOwnerException()
  {
    // Given
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);
    var exceptionMessage = "Você precisa ser o dono do vídeo pra poder altera-lo";

    _moqService.Setup(x => x.UpdateVideoAsync(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>(), It.IsAny<string>())).Throws(new NotTheOwnerException(exceptionMessage));
    // When
    var result = await _controller.UpdateVideo(It.IsAny<int>(), It.IsAny<UpdateVideoDTO>()) as ObjectResult;

    // Then
    Assert.NotNull(result);
    Assert.Equal(StatusCodes.Status405MethodNotAllowed, result.StatusCode);
    Assert.Equal(exceptionMessage, result.Value);
  }
  #endregion

  #region DELETE
  [Fact]
  public async Task DeleteVideo_ReturnsInternalError_WhenExtractIdFromTokenFailAsync()
  {
    // Arrange
    var videoDto = new CreateVideoDto();
    var headers = new HeaderDictionary();
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    var expectedErrorMessage = "Erro ao extrair o id";

    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Throws(new ErrorToGetUserIdException(expectedErrorMessage));

    // Act
    var result = await _controller.DeleteVideo(It.IsAny<int>()) as ObjectResult;

    // Assert
    _tokenServiceMock.Verify(t => t.ExtractID(headers["Authorization"]), Times.Once());
    _tokenServiceMock.VerifyNoOtherCalls();
    Assert.NotNull(result);
    Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
    Assert.Equal(expectedErrorMessage, result.Value);
  }

  [Fact]
  public async void DeleteVideo_ReturnNotFound_WhenServiceThrowsNullReferenceException()
  {
    // Given
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);

    string errorMessage = "Não encontrado";
    _moqService.Setup(x => x.DeleteVideoAsync(It.IsAny<int>(), It.IsAny<string>())).Throws(new NullReferenceException(errorMessage));

    // When
    var response = await _controller.DeleteVideo(It.IsAny<int>()) as ObjectResult;

    // Then
    _tokenServiceMock.Verify(t => t.ExtractID(headers["Authorization"]), Times.Once());
    _tokenServiceMock.VerifyNoOtherCalls();
    Assert.NotNull(response);
    Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    Assert.Equal(errorMessage, response.Value);
  }

  [Fact]
  public async void DeleteVideo_ReturnMethodNotAllowed_WhenVideoServiceThrowNotTheVideoOwnerException()
  {
    // Given
    var headers = new HeaderDictionary();
    string idUser = "user123";
    headers.Add("Authorization", "Bearer [suppose_to_be_a_valid_token]");
    _tokenServiceMock.Setup(t => t.ExtractID(headers["Authorization"])).Returns(idUser);
    var exceptionMessage = "Você precisa ser o dono do vídeo pra poder altera-lo";

    _moqService.Setup(x => x.DeleteVideoAsync(It.IsAny<int>(), It.IsAny<string>())).Throws(new NotTheOwnerException(exceptionMessage));

    // When
    var result = await _controller.DeleteVideo(It.IsAny<int>()) as ObjectResult;

    // Then
    Assert.NotNull(result);
    Assert.Equal(StatusCodes.Status405MethodNotAllowed, result.StatusCode);
    Assert.Equal(exceptionMessage, result.Value);
  }

  [Fact]
  public async void TestDeleteVideoReturnNoContent()
  {
    // Given
    Result result = Result.Ok();
    _moqService.Setup(x => x.DeleteVideoAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(result));
    // When
    var response = await _controller.DeleteVideo(It.IsAny<int>());
    // Then
    Assert.IsType<NoContentResult>(response);
  }
  #endregion 

}
