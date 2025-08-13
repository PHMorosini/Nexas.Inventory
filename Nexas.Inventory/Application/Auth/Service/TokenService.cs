using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nexas.Inventory.Application.Auth.Interface;
using Nexas.Inventory.Application.User.ViewModel;
using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.JwtSettings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nexas.Inventory.Application.Auth.Service;
public class TokenService : ITokenService
{
    private readonly Jwt _jwtSettings;

    public TokenService(IOptions<Jwt> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Task<string> GenerateToken(UserViewModel user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenString);
    }
}


