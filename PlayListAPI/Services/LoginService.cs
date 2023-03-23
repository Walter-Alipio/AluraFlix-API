using FluentResults;
using Microsoft.AspNetCore.Identity;
using PlayListAPI.Requests;

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

  public Result UserLogin(LoginRequest loginRequest)
  {
    var identity = _signInManager
        .PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, false, false);

    if (identity.Result.Succeeded)
    {
      var identityUser = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == loginRequest.UserName.ToUpper());

      var token = _token.CreateToken(identityUser!, _signInManager
          .UserManager.GetRolesAsync(identityUser).Result.FirstOrDefault());

      return Result.Ok().WithSuccess(token.Value);
    }

    return Result.Fail("Login falhou!");
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