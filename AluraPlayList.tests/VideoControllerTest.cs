using AluraPlayList.Data.DTOs.VideosDTOs;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AluraPlayList.tests
{
  public class VideoControllerTest
  {
    private FakeVideoService _fakeVideoServide;
    private VideosController _videoController;
    public VideoControllerTest()
    {
      _fakeVideoServide = new FakeVideoService();
      _videoController = new VideosController(_fakeVideoServide);
    }


    [Fact]
    public void CreateNewVideoWithGoodParamsShouldReturnCreated()
    {
      //Arrange
      CreateVideoDto videoDto = new()
      {
        Title = "Video teste",
        Description = "video algum",
        Url = "www.video.com",
        CategoriaId = 1
      };

      //Act
      var response = _videoController.addVideo(videoDto);
      //Assert
      Assert.IsType<CreatedResult>(response);
    }

    [Fact]
    public void CreateNewVideoBadParamsShouldReturnBadRequest()
    {
      //Arrange
      CreateVideoDto videoDto = new();
      //Act
      var response = _videoController.addVideo(videoDto);
      //Assert
      Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public void ShowVideoByExistentIdShouldReturnOk()
    {

      //Act
      var response = _videoController.showVideoById(1);
      //Assert
      Assert.IsType<OkObjectResult>(response);
    }
    [Fact]
    public void ResultOfShowVideoByInexistentIdShouldReturnNotFound()
    {
      //Act
      var response = _videoController.showVideoById(0);
      //Assert
      Assert.IsNotType<NotFoundObjectResult>(response);
    }
    [Fact]
    public void ShowAllVideosShouldReturnOk()
    {
      //Act
      var response = _videoController.showAllVideos("");
      //Assert
      Assert.IsType<OkObjectResult>(response);
    }
    [Fact]
    public void ShowAllVideosByInexistentTitleShouldReturnNotFound()
    {
      //Act
      var response = _videoController.showAllVideos("Jóse Saramago");
      //Assert
      Assert.IsType<NotFoundResult>(response);
    }
    [Fact]
    public void DeleteVideoShouldReturnNoContent()
    {
      //Act
      var response = _videoController.deleteVideo(1);
      //Assert
      Assert.IsType<NoContentResult>(response);
    }
    [Fact]
    public void DeleteVideoWithInexistentIdShouldReturnNotFound()
    {
      //Act
      var response = _videoController.deleteVideo(15);
      //Assert
      Assert.IsType<NotFoundResult>(response);
    }
    [Fact]
    public void UpdateVideoByIdShouldReturnCreateAction()
    {
      //Arrange
      UpdateVideoDTO updateVideoDTO = new()
      {
        Title = "Video teste",
        Description = "video algum",
        Url = "www.video.com",
        CategoriaId = 1
      };
      //Act
      var response = _videoController.updateVideo(1, updateVideoDTO);
      //Assert
      Assert.IsType<CreatedAtActionResult>(response);
    }
    [Fact]
    public void UpdateVideoWithInexistentIdShouldReturnNotFound()
    {
      //Arrange
      UpdateVideoDTO updateVideoDTO = new()
      {
        Title = "Video teste",
        Description = "video algum",
        Url = "www.video.com",
        CategoriaId = 1
      };
      //Act
      var response = _videoController.updateVideo(15, updateVideoDTO);
      //Assert
      Assert.IsType<NotFoundResult>(response);
    }
    [Fact]
    public void UpdateVideoWithInvalidUrlShouldReturnBadRequest()
    {
      //Arrange
      UpdateVideoDTO updateVideoDTO = new()
      {
        Title = "Video teste",
        Description = "video algum",
        Url = "www",
        CategoriaId = 1
      };
      //Act
      var response = _videoController.updateVideo(1, updateVideoDTO);
      //Assert
      Assert.IsType<BadRequestObjectResult>(response);
    }

  }
}