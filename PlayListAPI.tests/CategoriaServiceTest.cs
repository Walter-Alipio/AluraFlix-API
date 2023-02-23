using AutoMapper;
using Moq;
using PlayListAPI.Data.DTOs.CategoriasDTOs;
using PlayListAPI.Models;
using PlayListAPI.Profiles;
using PlayListAPI.Repository;
using PlayListAPI.Services;
using PlayListAPI.Services.Interfaces;

public class CategoriaServiceTest
{
    private Mock<ICategoriaRepository> _MockRepository = new Mock<ICategoriaRepository>();
    private ICategoriaService _service;
    public CategoriaServiceTest()
    {
        //To use the real map function
        var profile = new CategoriasProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        IMapper mapper = new Mapper(configuration);

        _service = new CategoriaService(mapper, _MockRepository.Object);
    }

    [Fact]
    public async void TestShowAllCategoriasAsyncReturnEmptytList()
    {
        // Given
        List<Categoria> categorias = new List<Categoria>();
        _MockRepository.Setup(d => d.GetAll(c => c.Videos)).Returns(Task.FromResult(categorias));
        // When
        var result = await _service.ShowAllCategoriasAsync();
        // Then
        Assert.Empty(result);
    }

    [Fact]
    public async void TestShowCategoriaByIdAsyncReturnNull()
    {
        // Given
        int id = 1;
        _MockRepository.Setup(d => d.GetByIdAsync(id, c => c.Videos)).Returns(Task.FromResult<Categoria?>(null));
        // When
        var result = await _service.ShowCategoriaByIdAsync(id);
        // Then
        Assert.Null(result);
    }

    [Fact]
    public async void TestShowVideosByCategoriaIdAsyncReturnEmptyList()
    {
        // Given
        int id = 1;
        List<Video> videos = new();
        _MockRepository.Setup(d => d.GetVideosByCategoriaAsync(id)).Returns(Task.FromResult(videos));

        // When
        var result = await _service.ShowVideosByCategoriaIdAsync(id);
        // Then
        Assert.Empty(result);
    }

    [Theory]
    [InlineData("#123123")]
    [InlineData("#ffffff")]
    [InlineData("#FFFFFF")]
    [InlineData("#123fff")]
    [InlineData("#12fF23")]
    public async void TestAddCategoriaAsyncReturnReadCategoriasDto(string color)
    {
        // Given
        CreateCategoriasDto dto = new()
        {
            Title = "Divertido",
            Cor = color
        };

        // When
        var result = await _service.AddCategoriaAsync(dto);
        // Then
        Assert.IsType<ReadCategoriasDto>(result);
    }
    [Fact]
    public async void TestUpdateCategoriaAsyncReturnFailed()
    {
        // Given
        int id = 1;
        UpdateCategoriasDtos dto = new();

        _MockRepository.Setup(d => d.GetByIdAsync(id, c => c.Videos)).Returns(Task.FromResult<Categoria?>(null));

        // When
        var result = await _service.UpdateCategoriaAsync(id, dto);
        // Then
        Assert.Null(result);
    }

    [Fact]
    public async void TestDeleteCategoriasAsyncReturnResultFailed()
    {
        // Given
        int id = 1;
        _MockRepository.Setup(d => d.GetByIdAsync(id, c => c.Videos)).Returns(Task.FromResult<Categoria?>(null));
        // When
        var result = await _service.DeleteCategoriasAsync(id);
        // Then
        Assert.True(result.IsFailed);
    }

}