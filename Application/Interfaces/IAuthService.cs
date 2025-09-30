using Application.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<IdentityResult> CreateUser(RegisterUserDto dto);
    Task<IdentityResult> UpdateUser(string userId, UpdateUserDto dto);
    Task<bool> DeleteUser(string userId);
    Task<LoginResponseDto> Authenticate(LoginRequestDto dto);
}

