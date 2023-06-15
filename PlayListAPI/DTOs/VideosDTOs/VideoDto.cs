namespace PlayListAPI.DTOs.VideosDTOs;
public class VideoDto
{

  public virtual string? Title { get; set; }
  public virtual string? Description { get; set; }
  public virtual string? Url { get; set; }
  public virtual int? CategoriaId { get; set; }

}
