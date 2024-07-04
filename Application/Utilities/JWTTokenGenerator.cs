
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Utilities;

public static class JWTTokenGenerator
{
    public static string GenerateJsonWebToken(this User user, AppSettings appSettings)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWTOptions.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim(ClaimTypes.Role, user.RoleName),
        };
        var token = new JwtSecurityToken(
           issuer: appSettings.JWTOptions.Issuer,
           audience: appSettings.JWTOptions.Audience,
           claims,
           expires: DateTime.UtcNow.AddMinutes(120),
           signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}