namespace PlayListAPI.Data.DTOs.TokenDTOs;

public class TokenDto
{
    public string Value { get; }

    public TokenDto(string value)
    {
        Value = value;
    }
}