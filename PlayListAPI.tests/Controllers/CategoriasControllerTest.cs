using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlayListAPI.Controllers;
using PlayListAPI.DTOs.CategoriasDTOs;
using PlayListAPI.DTOs.VideosDTOs;
using PlayListAPI.Services.Interfaces;

namespace PlayListAPI.tests;

public class CategoriasControllerTest
{
  private Mock<ICategoriaService> _moqService = new Mock<ICategoriaService>();
  private CategoriasController _controller;
  public CategoriasControllerTest()
  {
    _controller = new CategoriasController(_moqService.Object);
  }

  [Fact]
  public async void AddCategoria_ReturnsBadRequest_WhenDtoIsNull()
  {
    // Given
    CreateCategoriasDto createDto = new CreateCategoriasDto();
    // When
    var response = await _controller.AddCategoria(createDto);
    // Then
    Assert.IsType<BadRequestObjectResult>(response);
  }

  [Fact]
  public async void AddCategoria_ReturnsCreated_WhenTheOperationIsSuccess()
  {
    // Given
    CreateCategoriasDto createDto = new CreateCategoriasDto();
    ReadCategoriasDto readDto = new ReadCategoriasDto();

    _moqService.Setup(x => x.AddCategoriaAsync(createDto))
      .Returns(Task.FromResult<ReadCategoriasDto?>(readDto));
    // When
    var response = await _controller.AddCategoria(createDto);
    // Then
    Assert.IsType<CreatedAtActionResult>(response);
  }

  [Fact]
  public async void ShowCategoriaById_ReturnsNotFound_WhenDtoIsNull()
  {
    // Given
    // When
    var response = await _controller.ShowCategoriaById(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void ShowCategoriaById_ReturnsOk_WhenTheOperationIsSuccess()
  {
    // Given
    ReadCategoriasDto readDto = new ReadCategoriasDto();

    _moqService.Setup(x => x.ShowCategoriaByIdAsync(1))
      .Returns(Task.FromResult<ReadCategoriasDto?>(readDto));
    // When
    var response = await _controller.ShowCategoriaById(1);
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public async void ShowVideosByCategoriaId_ReturnsNotFound_WhenDtoListIsEmpty()
  {
    // Given
    _moqService.Setup(x => x.ShowVideosByCategoriaIdAsync(1)).Returns(Task.FromResult(new List<ReadVideoDTO>()));
    // When
    var response = await _controller.ShowVideosByCategoriaId(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void ShowVideosByCategoriaId_ReturnsSelectedVideo_WhenTheOperationIsSuccess()
  {
    // Given
    List<ReadVideoDTO> readDto = new()
      {
        new ReadVideoDTO(),
        new ReadVideoDTO()
      };

    _moqService.Setup(x => x.ShowVideosByCategoriaIdAsync(1))
      .Returns(Task.FromResult(readDto));
    // When
    var response = await _controller.ShowVideosByCategoriaId(1);
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public async void ShowAllCategorias_ReturnsNotFound_WhenDtoListIsEmpty()
  {
    // Given
    List<ReadCategoriasDto> categoriasDtos = new();

    _moqService.Setup(x => x.ShowAllCategoriasAsync())
      .Returns(Task.FromResult(categoriasDtos));

    // When
    var response = await _controller.ShowAllCategorias();
    // Then
    Assert.IsType<NotFoundObjectResult>(response);
  }

  [Fact]
  public async void ShowAllCategorias_ReturnsCategoriasList_WhenTheOperationIsSucess()
  {
    // Given
    List<ReadCategoriasDto> categoriasDtos = new()
      {
        new ReadCategoriasDto()
      };

    _moqService.Setup(x => x.ShowAllCategoriasAsync())
      .Returns(Task.FromResult(categoriasDtos));

    // When
    var response = await _controller.ShowAllCategorias();
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public async void UpdateCategoria_ReturnsNotFound_WhenDtoIsEmpty()
  {
    // Given
    UpdateCategoriasDtos updateDto = new();
    // When
    var response = await _controller.UpdateCategoria(1, updateDto);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void UpdateCategoria_ReturnsCreated_WhenOperationIsSuccess()
  {
    // Given
    UpdateCategoriasDtos updateDto = new();
    ReadCategoriasDto readDto = new();

    _moqService.Setup(x => x.UpdateCategoriaAsync(1, updateDto))
      .Returns(Task.FromResult<ReadCategoriasDto?>(readDto));
    // When

    var response = await _controller.UpdateCategoria(1, updateDto);
    // Then
    Assert.IsType<CreatedAtActionResult>(response);
  }

  [Fact]
  public async void DeleteCategoria_ReturnsNotFound_WhenDeleteReturnsNullException()
  {
    // Given
    _moqService.Setup(x => x.DeleteCategoriasAsync(1)).Throws(new NullReferenceException());

    // When
    var response = await _controller.DeleteCategorias(1);

    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void DeleteCategoria_ReturnsNoContent_WhenOperationIsSuccess()
  {
    // Given
    Result result = Result.Ok();
    _moqService.Setup(x => x.DeleteCategoriasAsync(1)).Returns(Task.FromResult(result));
    // When
    var response = await _controller.DeleteCategorias(1);
    // Then
    Assert.IsType<NoContentResult>(response);
  }


}