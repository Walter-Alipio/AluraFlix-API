using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PlayListAPI.Data.DTOs.RegisterDTOs;

namespace PlayListAPI.Services;

public class RegisterService
{
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterService(IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public bool RegisterUser(RegisterUserDto registerUser)
    {
        IdentityUser identityUser = _mapper.Map<IdentityUser>(registerUser);
        var resultIdentity = _userManager.CreateAsync(identityUser, registerUser.Password);

        if (resultIdentity.Result.Succeeded) return true;

        return false;
    }
}