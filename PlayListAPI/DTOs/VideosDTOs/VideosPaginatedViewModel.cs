using PlayListAPI.DTOs.VideosDTOs;

public class VideosPaginatedViewModel
{
  public string Total { get; set; } = string.Empty;
  public string Next { get; set; } = string.Empty;
  public string Previews { get; set; } = string.Empty;
  public List<ReadVideoDTO>? Videos { get; set; }
}