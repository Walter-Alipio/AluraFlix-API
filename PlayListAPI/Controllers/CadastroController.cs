using Microsoft.AspNetCore.Mvc;
using PlayListAPI.Data.DTOs.RegisterDTOs;
using PlayListAPI.Services;

namespace PlayListAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class CadastroController : ControllerBase
{
    private RegisterService _registerService;

    public CadastroController(RegisterService registerService)
    {
        _registerService = registerService;
    }

    [HttpPost]
    public IActionResult RegisterUser(RegisterUserDto registerUser)
    {
        bool result = _registerService.RegisterUser(registerUser);
        if (!result) return StatusCode(500);

        return Ok();
    }
}