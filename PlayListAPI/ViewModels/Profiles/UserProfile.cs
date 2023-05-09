using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PlayListAPI.Data.DTOs.RegisterDTOs;

namespace PlayListAPI.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserDto, IdentityUser>();
    }
}