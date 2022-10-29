using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

public class CreateVideoDto
{
  [Required(ErrorMessage = "Campo Titulo é obrigaório.")]
  [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
  public string? Title { get; set; }


  [Required(ErrorMessage = "Campo Descrição é obrigaório.")]
  [StringLength(5000, ErrorMessage = "Tamanho máximo do campo 5000 caracteres")]
  public string? Description { get; set; }

  private string _urlCheck = "https://www.youtube.com/watch?v";

  [Required(ErrorMessage = "Campo URL é obrigaório.")]
  [StringLength(100, ErrorMessage = "Tamanho máximo do campo 100 caracteres")]
  public string Url { get; set; }

  public void urlTest()
  {
    string[] url = this.Url.Split("=");
    if (!url[0].Equals(value: _urlCheck) || url[1].Length != 11)
    {
      throw new Exception(
        @"Url inválida = Formato: https://www.youtube.com/watch?v=###########"
        );
    }
  }

}