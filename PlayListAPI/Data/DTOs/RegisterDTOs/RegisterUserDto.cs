using System.ComponentModel.DataAnnotations;

namespace PlayListAPI.Data.DTOs.RegisterDTOs;
public class RegisterUserDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

}