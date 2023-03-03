using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PlayListAPI.Data.DTOs.RegisterDTOs;

namespace PlayListAPI.Services;

public class RegisterService
{
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterService(IMapper mapper, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> RegisterUser(RegisterUserDto registerUser)
    {
        IdentityUser identityUser = _mapper.Map<IdentityUser>(registerUser);
        var resultIdentity = await _userManager.CreateAsync(identityUser, registerUser.Password);
        var createRoleResult = _roleManager.CreateAsync(new IdentityRole("user")).Result;
        var userRoleResult = _userManager.AddToRoleAsync(identityUser, "user").Result;

        if (resultIdentity.Succeeded) return true;

        return false;
    }
}