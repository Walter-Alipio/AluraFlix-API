using System.ComponentModel.DataAnnotations;

namespace AluraPlayList.Models
{
  public class Categoria
  {
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo Titulo é obrigaório.")]
    [StringLength(50, ErrorMessage = "Tamanho máximo do campo 50 caracteres.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Campo cor é obrigatório.")]
    [StringLength(10, ErrorMessage = "Tamanho excedido.")]
    public string Cor { get; set; }
    public virtual List<Video> Videos { get; set; }
  }
}