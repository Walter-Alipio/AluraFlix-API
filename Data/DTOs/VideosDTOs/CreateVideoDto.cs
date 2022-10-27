using System.ComponentModel.DataAnnotations;

public class CreateVideoDto
{
  [Required(ErrorMessage = "Campo Titulo é obrigaório.")]
  [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
  public string Title { get; set; }


  [Required(ErrorMessage = "Campo Descrição é obrigaório.")]
  [StringLength(5000, ErrorMessage = "Tamanho máximo do campo 5000 caracteres")]
  public string Description { get; set; }



  [Required(ErrorMessage = "Campo URL é obrigaório.")]
  [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
  public string Url { get; set; }
}