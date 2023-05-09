using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlayListAPI.Controllers;
using PlayListAPI.Data.DTOs.CategoriasDTOs;
using PlayListAPI.Data.DTOs.VideosDTOs;
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
  public async void TestAddCategoriaReturnsBadRequest()
  {
    // Given
    CreateCategoriasDto createDto = new CreateCategoriasDto();
    // When
    var response = await _controller.AddCategoria(createDto);
    // Then
    Assert.IsType<BadRequestObjectResult>(response);
  }

  [Fact]
  public async void TestAddCategoriaReturnsCreated()
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
  public async void TestShowCategoriaByIdReturnsNotFound()
  {
    // Given
    // When
    var response = await _controller.ShowCategoriaById(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void TestShowCategoriaByIdReturnsOk()
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
  public async void TestShowVideosByCategoriaIdReturnsNotFound()
  {
    // Given
    // When
    var response = await _controller.ShowVideosByCategoriaId(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void TestShowVideosByCategoriaIdReturnsOk()
  {
    // Given
    List<ReadVideoDTO> readDto = new();

    _moqService.Setup(x => x.ShowVideosByCategoriaIdAsync(1))
      .Returns(Task.FromResult(readDto));
    // When
    var response = await _controller.ShowVideosByCategoriaId(1);
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public async void TestShowAllCategoriasReturnsNotFound()
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
  public async void TestShowAllCategoriasReturnsOk()
  {
    // Given
    ReadCategoriasDto readDto = new ReadCategoriasDto();
    List<ReadCategoriasDto> categoriasDtos = new();
    categoriasDtos.Add(readDto);
    _moqService.Setup(x => x.ShowAllCategoriasAsync())
      .Returns(Task.FromResult(categoriasDtos));

    // When
    var response = await _controller.ShowAllCategorias();
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public async void TestUpdateCategoriaReturnsNotFound()
  {
    // Given
    UpdateCategoriasDtos updateDto = new();
    // When
    var response = await _controller.UpdateCategoria(1, updateDto);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void TestUpdateCategoriaReturnsCreated()
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
  public async void TestDeleteCategoriaReturnsNotFound()
  {
    // Given
    Result result = Result.Fail("Não encontrado");
    _moqService.Setup(x => x.DeleteCategoriasAsync(1)).Returns(Task.FromResult(result));
    // When
    var response = await _controller.DeleteCategorias(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public async void TestDeleteCategoriaReturnsNoContent()
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