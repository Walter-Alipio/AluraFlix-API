using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PlayListAPI.DTOs.LoginDTOs;
using PlayListAPI.Services;

namespace PlayListAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
  private readonly LoginService _loginService;

  public LoginController(LoginService loginService)
  {
    _loginService = loginService;
  }

  [HttpPost("/User/Login")]
  public IActionResult UserLogin(LoginRequestDto loginRequest)
  {
    try
    {
      var result = _loginService.UserLogin(loginRequest);
      return Ok(result);

    }
    catch (FailToLoginException e)
    {

      return Unauthorized(e.Message);
    }

  }

  // [HttpPost("/User/Logout")]
  // public IActionResult UserLogout()
  // {
  //   Result result;
  //   try
  //   {
  //     result = _loginService.Logout();
  //   }
  //   catch (System.Exception e)
  //   {

  //     return StatusCode(500, e);
  //   }

  //   if (result.IsFailed) return Unauthorized(result.Errors.First());

  //   return Ok();
  // }

}