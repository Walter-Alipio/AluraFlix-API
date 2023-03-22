using PlayListAPI.Data.DTOs.VideosDTOs;

public class VideosPaginated
{
  public string Total { get; set; }
  public string? Next { get; set; }
  public string? Previews { get; set; }
  public List<ReadVideoDTO> Videos { get; set; }
}