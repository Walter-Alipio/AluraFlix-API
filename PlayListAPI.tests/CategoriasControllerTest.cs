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
  private Mock<ICategoriasService> _moqService = new Mock<ICategoriasService>();
  private CategoriasController _controller;
  public CategoriasControllerTest()
  {
    _controller = new CategoriasController(_moqService.Object);
  }

  [Fact]
  public void TestAddCategoriaReturnsBadRequest()
  {
    // Given
    CreateCategoriasDto createDto = new CreateCategoriasDto();
    // When
    var response = _controller.AddCategoria(createDto);
    // Then
    Assert.IsType<BadRequestObjectResult>(response);
  }

  [Fact]
  public void TestAddCategoriaReturnsCreated()
  {
    // Given
    CreateCategoriasDto createDto = new CreateCategoriasDto();
    ReadCategoriasDto readDto = new ReadCategoriasDto();

    _moqService.Setup(x => x.AddCategoria(createDto)).Returns(readDto);
    // When
    var response = _controller.AddCategoria(createDto);
    // Then
    Assert.IsType<CreatedAtActionResult>(response);
  }

  [Fact]
  public void TestShowCategoriaByIdReturnsNotFound()
  {
    // Given
    // When
    var response = _controller.ShowCategoriaById(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public void TestShowCategoriaByIdReturnsOk()
  {
    // Given
    ReadCategoriasDto readDto = new ReadCategoriasDto();

    _moqService.Setup(x => x.ShowCategoriaById(1)).Returns(readDto);
    // When
    var response = _controller.ShowCategoriaById(1);
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public void TestShowVideosByCategoriaIdReturnsNotFound()
  {
    // Given
    // When
    var response = _controller.ShowVideosByCategoriaId(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public void TestShowVideosByCategoriaIdReturnsOk()
  {
    // Given
    List<ReadVideoDTO> readDto = new();

    _moqService.Setup(x => x.ShowVideosByCategoriaId(1)).Returns(readDto);
    // When
    var response = _controller.ShowVideosByCategoriaId(1);
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public void TestShowAllCategoriasReturnsNotFound()
  {
    // Given
    List<ReadCategoriasDto> categoriasDtos = new();
    _moqService.Setup(x => x.ShowAllCategorias()).Returns(categoriasDtos);
    // When
    var response = _controller.ShowAllCategorias();
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public void TestShowAllCategoriasReturnsOk()
  {
    // Given
    ReadCategoriasDto readDto = new ReadCategoriasDto();
    List<ReadCategoriasDto> categoriasDtos = new();
    categoriasDtos.Add(readDto);
    _moqService.Setup(x => x.ShowAllCategorias()).Returns(categoriasDtos);
    // When
    var response = _controller.ShowAllCategorias();
    // Then
    Assert.IsType<OkObjectResult>(response);
  }

  [Fact]
  public void TestUpdateCategoriaReturnsNotFound()
  {
    // Given
    UpdateCategoriasDtos updateDto = new();
    // When

    var response = _controller.UpdateCategoria(1, updateDto);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public void TestUpdateCategoriaReturnsCreated()
  {
    // Given
    UpdateCategoriasDtos updateDto = new();
    ReadCategoriasDto readDto = new();

    _moqService.Setup(x => x.UpdateCategoria(1, updateDto)).Returns(readDto);
    // When

    var response = _controller.UpdateCategoria(1, updateDto);
    // Then
    Assert.IsType<CreatedAtActionResult>(response);
  }

  [Fact]
  public void TestDeleteCategoriaReturnsNotFound()
  {
    // Given
    Result result = Result.Fail("Não encontrado");
    _moqService.Setup(x => x.DeleteCategorias(1)).Returns(result);
    // When
    var response = _controller.DeleteCategorias(1);
    // Then
    Assert.IsType<NotFoundResult>(response);
  }

  [Fact]
  public void TestDeleteCategoriaReturnsNoContent()
  {
    // Given
    Result result = Result.Ok();
    _moqService.Setup(x => x.DeleteCategorias(1)).Returns(result);
    // When
    var response = _controller.DeleteCategorias(1);
    // Then
    Assert.IsType<NoContentResult>(response);
  }


}