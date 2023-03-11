using System.ComponentModel.DataAnnotations;

namespace PlayListAPI.Data.DTOs.VideosDTOs
{

  public class UpdateVideoDTO : VideoDto
  {

    [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
    public override string? Title { get; set; }

    [StringLength(5000, ErrorMessage = "Tamanho máximo do campo 5000 caracteres")]
    public override string? Description { get; set; }

    [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
    public override string? Url { get; set; }

    public override int? CategoriaId { get; set; } = null;
  }
}