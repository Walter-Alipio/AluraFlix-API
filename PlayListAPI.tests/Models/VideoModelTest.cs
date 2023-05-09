using PlayListAPI.Data.DTOs.VideosDTOs;
using PlayListAPI.Models;
using PlayListAPI.ViewModels.CustomMapper;

namespace PlayListAPI.tests.Models;

public class VideoModelTest
{
  [Fact]
  public void MapUpdateDtoToVideo_ReturnsUpdatedVideo_OnlyWithChangedValues()
  {
    // Given
    var videoDTO = new UpdateVideoDTO()
    {
      Title = "Livro do pavê - parte 1",
      Description = "",
      CategoriaId = null,
      Url = "http://www.youtube.com/"
    };

    var video = new Video()
    {
      Id = 1,
      Title = "Livro do pavê",
      Description = "A incrivel história do pavê de copo",
      CategoriaId = 5,
      Url = "http://www.twitte.com/"
    };

    var customMapper = new CustomMapVideo();


    // When
    customMapper.MapUpdateDtoToVideo(videoDTO, video);

    // Then
    Assert.Equal(videoDTO.Url, video.Url);
    Assert.Equal(videoDTO.Title, video.Title);
    Assert.NotEqual(videoDTO.Description, video.Description);
    Assert.NotEqual(videoDTO.CategoriaId, video.CategoriaId);
  }

  [Fact]
  public void MapUpdateDtoToVideo_ReturnsVideoWithoutChange_WhenUpdateDtoOnlyHaveEmptyNullAttributes()
  {
    // Given
    var videoDTO = new UpdateVideoDTO()
    {
      Title = "",
      Description = "",
      CategoriaId = null,
      Url = ""
    };

    var video = new Video()
    {
      Id = 1,
      Title = "Livro do pavê",
      Description = "A incrivel história do pavê de copo",
      CategoriaId = 5,
      Url = "http://www.twitte.com/"
    };

    var customMapper = new CustomMapVideo();


    // When
    customMapper.MapUpdateDtoToVideo(videoDTO, video);

    // Then
    Assert.NotEqual(videoDTO.Url, video.Url);
    Assert.NotEqual(videoDTO.Title, video.Title);
    Assert.NotEqual(videoDTO.Description, video.Description);
    Assert.NotEqual(videoDTO.CategoriaId, video.CategoriaId);
  }
}