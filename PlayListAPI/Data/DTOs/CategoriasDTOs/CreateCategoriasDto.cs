using System.ComponentModel.DataAnnotations;

namespace PlayListAPI.Data.DTOs.CategoriasDTOs
{
  public class CreateCategoriasDto
  {
    [Required(ErrorMessage = "Campo Titulo é obrigaório.")]
    [StringLength(50, ErrorMessage = "Tamanho máximo do campo 50 caracteres.")]
    public string Title { get; set; } = string.Empty;


    [Required(ErrorMessage = "Campo cor é obrigatório.")]
    [StringLength(10, ErrorMessage = "Tamanho excedido.")]
    public string Cor { get; set; } = string.Empty;
  }
}