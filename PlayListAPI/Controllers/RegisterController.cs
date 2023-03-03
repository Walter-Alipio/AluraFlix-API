using Microsoft.AspNetCore.Mvc;
using PlayListAPI.Data.DTOs.RegisterDTOs;
using PlayListAPI.Services;

namespace PlayListAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private RegisterService _registerService;

    public RegisterController(RegisterService registerService)
    {
        _registerService = registerService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegisterUserDto registerUser)
    {
        bool result = await _registerService.RegisterUser(registerUser);
        if (!result) return StatusCode(500);

        return Ok();
    }
}