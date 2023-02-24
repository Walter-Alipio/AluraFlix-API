using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PlayListAPI.Data.DTOs.TokenDTOs;

namespace PlayListAPI.Services;

public class TokenService
{
    public TokenDto CreateToken(IdentityUser user)
    {
        Claim[] userClaims = new Claim[]
        {
            new Claim("username", user.UserName),
            new Claim("id", user.Id.ToString())
        };

        var hash = new Guid();
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(hash.ToString())
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //create token
        var token = new JwtSecurityToken(
            claims: userClaims,
            signingCredentials: credentials,
            expires: DateTime.UtcNow.AddHours(1)
        );
        //converte token to string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenDto(tokenString);
    }
}