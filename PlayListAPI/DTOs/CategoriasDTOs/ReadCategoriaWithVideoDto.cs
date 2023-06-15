namespace PlayListAPI.DTOs.CategoriasDTOs;
public class ReadCategoriaWithVideoDto
{
  public int Id { get; set; }
  public string? Title { get; set; }
  public string? Cor { get; set; }
  public object? Videos { get; set; }
}
