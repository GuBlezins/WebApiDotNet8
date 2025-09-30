using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;


public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _UserManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<IdentityUser> UserManager, IConfiguration configuration)
    {
        _UserManager = UserManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> CreateUser(RegisterUserDto dto)
    {
        var User = new IdentityUser
        {
            UserName = dto.Username,
            Email = dto.Email
        };
        return await _UserManager.CreateAsync(User, dto.Password);
    }

    public async Task<IdentityResult> UpdateUser(string UserId, UpdateUserDto dto)
    {
        var User = await _UserManager.FindByIdAsync(UserId);
        if (User == null) throw new KeyNotFoundException("Usuário não encontrado.");

        if (!string.IsNullOrEmpty(dto.Username)) User.UserName = dto.Username;
        if (!string.IsNullOrEmpty(dto.Email)) User.Email = dto.Email;

        var result = await _UserManager.UpdateAsync(User);

        // Atualiza a senha se fornecida
        if (!string.IsNullOrEmpty(dto.Password))
        {
            var token = await _UserManager.GeneratePasswordResetTokenAsync(User);
            var passwordResult = await _UserManager.ResetPasswordAsync(User, token, dto.Password);
            if (!passwordResult.Succeeded) return passwordResult;
        }

        return result;
    }

    public async Task<bool> DeleteUser(string UserId)
    {
        var User = await _UserManager.FindByIdAsync(UserId);
        if (User == null) return false;

        var result = await _UserManager.DeleteAsync(User);
        return result.Succeeded;
    }

    public async Task<IdentityUser?> GetUserById(string UserId)
    {
        return await _UserManager.FindByIdAsync(UserId);
    }


    public async Task<LoginResponseDto> Authenticate(LoginRequestDto dto)
    {
        var User = await _UserManager.FindByEmailAsync(dto.Email);
        if (User == null || !await _UserManager.CheckPasswordAsync(User, dto.Password))
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, User.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
            signingCredentials: creds
        );

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        };
    }

}