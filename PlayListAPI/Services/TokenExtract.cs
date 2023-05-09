using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Primitives;
using PlayListAPI.Exceptions;
using PlayListAPI.Services.Interfaces;

namespace PlayListAPI.Services;
public class TokenExtract : ITokenExtract
{
  public string ExtractID(StringValues authHeader)
  {
    try
    {
      var token = authHeader.First()?.Substring("Bearer ".Length).Trim();

      // Extrair as reivindicações do token JWT
      var handler = new JwtSecurityTokenHandler();
      var claims = handler.ReadJwtToken(token).Claims;

      var userIdClaim = claims.FirstOrDefault(c => c.Type == "id");

      if (userIdClaim is null) throw new ArgumentException();

      return userIdClaim.Value;
    }
    catch (System.Exception)
    {
      throw new ErrorToGetUserIdException("Um erro ocorreu ao tentar extrair o id do usuário.");
    }
  }
}