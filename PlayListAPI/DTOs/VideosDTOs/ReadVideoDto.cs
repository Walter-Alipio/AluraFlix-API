using PlayListAPI.Models;

namespace PlayListAPI.DTOs.VideosDTOs;

public class ReadVideoDTO
{
  public int Id { get; set; }

  public string? Title { get; set; }

  public string? Description { get; set; }

  public string? Url { get; set; }

  public virtual Categoria? Categoria { get; set; }
}
