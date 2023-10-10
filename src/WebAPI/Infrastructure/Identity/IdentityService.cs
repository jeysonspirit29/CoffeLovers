using Application.Common.Dtos.Auth;
using Application.Common.Interfaces;
using Azure;
using Domain.Domains.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtConfig _jwtSettings; 

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtConfig> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> Authenticate(AuthRequest request)
    {
        var authResponse = new AuthResponse();

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == request.UserName.ToLower());
        bool passwordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordCorrect)
        {
            authResponse.Message = IdentityErrors.UserOrPasswordInvalid;
            return authResponse;
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || !roles.Any())
        {
            authResponse.Message = IdentityErrors.NoRol;
            return authResponse;
        }

        var jwtSecurityToken = await GenerateJWToken(user, roles.First());
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        SetSuccessAuthResponse(authResponse, user, jwtToken, roles.First());

        return authResponse;
    }
    public async Task<ICollection<string>> GetUserIdsBySuperior(string chiefUserId)
    {
        return await _userManager.Users
                                    .Where(u => u.ChiefUserId == chiefUserId)
                                    .Select(x => x.Id)
                                    .ToListAsync();
    }
    private static void SetSuccessAuthResponse(AuthResponse authResponse, ApplicationUser user, string jwtToken, string rol)
    {
        authResponse.Success = true;
        authResponse.Id = user.Id;
        authResponse.UserName = user.UserName;
        authResponse.FullName = user.FullName;
        authResponse.Rol = rol;
        authResponse.Token = jwtToken;
    }

    private Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user, string rol)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, rol)
        };

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return Task.FromResult(jwtSecurityToken);
    }

}
