using System.Net;
using AluraPlayList.Data.DTOs.VideosDTOs;
using AluraPlayList.Services.Interfaces;
using FluentResults;
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
      CreateVideoDto videoDto = new()
      {
        Title = "Video teste",
        Description = "video algum",
        Url = "www.video.com",
        CategoriaId = 1
      };
      var response = _videoController.addVideo(videoDto);

      //Act
      Assert.IsType<CreatedResult>(response);
    }

    [Fact]
    public void CreateNewVideoBadParamsShouldReturnBadRequest()
    {
      CreateVideoDto videoDto = new();
      var response = _videoController.addVideo(videoDto);
      //Act
      Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public void ShowVideoByExistentIdShouldReturnOk()
    {
      var response = _videoController.showVideoById(1);
      //Act
      Assert.IsType<OkObjectResult>(response);
    }
    [Fact]
    public void ResultOfShowVideoByInexistentIdShouldReturnNotFound()
    {
      var response = _videoController.showVideoById(0);

      Assert.IsNotType<NotFoundObjectResult>(response);
    }
    [Fact]
    public void ShowAllVideosShouldReturnOk()
    {
      var response = _videoController.showAllVideos("");
      //Act
      Assert.IsType<OkObjectResult>(response);
    }
    [Fact]
    public void ShowAllVideosByInexistentTitleShouldReturnNotFound()
    {
      var response = _videoController.showAllVideos("Jóse Saramago");
      //Act
      Assert.IsType<NotFoundResult>(response);
    }
    [Fact]
    public void DeleteVideoShouldReturnNoContent()
    {
      var response = _videoController.deleteVideo(1);
      //Act
      Assert.IsType<NoContentResult>(response);
    }
    [Fact]
    public void DeleteVideoWithInexistentIdShouldReturnNotFound()
    {
      var response = _videoController.deleteVideo(15);
      //Act
      Assert.IsType<NotFoundResult>(response);
    }
    [Fact]
    public void UpdateVideoByIdShouldReturnCreateAction()
    {
      UpdateVideoDTO updateVideoDTO = new()
      {
        Title = "Video teste",
        Description = "video algum",
        Url = "www.video.com",
        CategoriaId = 1
      };
      var response = _videoController.updateVideo(1, updateVideoDTO);
      //Act
      Assert.IsType<CreatedAtActionResult>(response);
    }
    [Fact]
    public void UpdateVideoWithInexistentIdShouldReturnNotFound()
    {
      UpdateVideoDTO updateVideoDTO = new()
      {
        Title = "Video teste",
        Description = "video algum",
        Url = "www.video.com",
        CategoriaId = 1
      };
      var response = _videoController.updateVideo(15, updateVideoDTO);
      //Act
      Assert.IsType<NotFoundResult>(response);
    }

  }

  public class FakeVideoService : IVideosService
  {
    private List<ReadVideoDTO> _readVideo = new List<ReadVideoDTO>{
      new ReadVideoDTO(){
        Categoria = new Models.Categoria(){ Id=1, Title="Ação", Cor="Laranja"},
        Description = "Novo video",
        Id = 1,
        Title = "Video 1",
        Url = "www.video.com"
      },new ReadVideoDTO(){
        Categoria = new Models.Categoria(){ Id=1, Title="Ação", Cor="Laranja"},
        Description = "Novo video",
        Id = 1,
        Title = "Video 1",
        Url = "www.video.com"
      },
      new ReadVideoDTO(){
        Categoria = new Models.Categoria(){ Id=1, Title="Ação", Cor="Laranja"},
        Description = "Novo video",
        Id = 2,
        Title = "Video 1",
        Url = "www.video.com"
      },
      new ReadVideoDTO(){
        Categoria = new Models.Categoria(){ Id=1, Title="Ação", Cor="Laranja"},
        Description = "Novo video",
        Id = 3,
        Title = "Video 1",
        Url = "www.video.com"
      },
      new ReadVideoDTO(){
        Categoria = new Models.Categoria(){ Id=1, Title="Ação", Cor="Laranja"},
        Description = "Novo video",
        Id = 4,
        Title = "Video 1",
        Url = "www.video.com"
      }
    };
    public Result addVideo(CreateVideoDto videoDto)
    {
      if (String.IsNullOrEmpty(videoDto.Title))
      {
        return Result.Fail("Item nulo");
      }
      return Result.Ok();

    }

    public Result DeleteVideo(int id)
    {
      ReadVideoDTO readDto = GetVideoById(id);

      return readDto == null ? Result.Fail("Não encontrado") : Result.Ok();
    }

    public List<ReadVideoDTO> ShowAllVideos(string? videoTitle)
    {
      if (!String.IsNullOrEmpty(videoTitle))
      {
        ReadVideoDTO videoFound = _readVideo.Find(video => video.Title == videoTitle);
        if (videoFound == null)
          return null;
      }
      return _readVideo;
    }

    public ReadVideoDTO ShowVideoById(int id)
    {
      ReadVideoDTO readDto = GetVideoById(id);

      return readDto == null ? null : readDto;
    }

    private ReadVideoDTO GetVideoById(int id)
    {
      return _readVideo.Find(ob => ob.Id == id);
    }

    public ReadVideoDTO UpdateVideo(int id, UpdateVideoDTO videoDTO)
    {
      ReadVideoDTO readDto = GetVideoById(id);

      return readDto == null ? null : readDto;
    }
  }

}