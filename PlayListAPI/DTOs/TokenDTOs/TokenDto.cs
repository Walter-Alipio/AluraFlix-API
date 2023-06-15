namespace PlayListAPI.DTOs.TokenDTOs;

public class TokenDto
{
  public string Value { get; } = string.Empty;

  public TokenDto(string value)
  {
    Value = value;
  }
}