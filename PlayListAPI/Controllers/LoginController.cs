using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PlayListAPI.Requests;
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
  public IActionResult UserLogin(LoginRequest loginRequest)
  {
    Result result = _loginService.UserLogin(loginRequest);

    if (result.IsFailed) return Unauthorized();

    return Ok(result.Successes.First());
  }

  [HttpPost("/User/Logout")]
  public IActionResult UserLogout()
  {
    Result result;
    try
    {
      result = _loginService.Logout();
    }
    catch (System.Exception e)
    {

      return StatusCode(500, e);
    }

    if (result.IsFailed) return Unauthorized(result.Errors.First());

    return Ok();
  }
}