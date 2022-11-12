using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace AluraPlayList.Data.DTOs.VideosDTOs
{
  public class CreateVideoDto : VideoDto
  {
    [Required(ErrorMessage = "Campo Titulo é obrigaório.")]
    [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
    public override string? Title { get; set; }


    [Required(ErrorMessage = "Campo Descrição é obrigaório.")]
    [StringLength(5000, ErrorMessage = "Tamanho máximo do campo 5000 caracteres")]
    public override string? Description { get; set; }

    [Required(ErrorMessage = "Campo URL é obrigaório.")]
    [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
    public override string? Url { get; set; }

    [Required(ErrorMessage = "Campo CategoriaId é obrigatório.")]
    public override int CategoriaId { get; set; }
  }
}