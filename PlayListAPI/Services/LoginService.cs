using FluentResults;
using Microsoft.AspNetCore.Identity;
using PlayListAPI.DTOs.LoginDTOs;

namespace PlayListAPI.Services;

public class LoginService
{
  private readonly SignInManager<IdentityUser> _signInManager;
  private TokenService _token;

  public LoginService(SignInManager<IdentityUser> signInManager, TokenService token)
  {
    _signInManager = signInManager;
    _token = token;
  }

  public string UserLogin(LoginRequestDto loginRequest)
  {
    var identity = _signInManager
        .PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, false, false);

    if (identity.Result.Succeeded)
    {
      var identityUser = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == loginRequest.UserName.ToUpper());
      if (identityUser is null)
      {
        throw new NullReferenceException();
      }

      var token = _token.CreateToken(identityUser, _signInManager
          .UserManager.GetRolesAsync(identityUser).Result.FirstOrDefault());

      return token.Value;
    }

    throw new FailToLoginException("Login falhou!");
  }

  public Result Logout()
  {
    try
    {
      var identityResult = _signInManager.SignOutAsync();

      if (identityResult.IsCompletedSuccessfully) return Result.Ok();
    }

    catch (System.Exception)
    {

      throw;
    }


    return Result.Fail("Logout falhou");
  }
}