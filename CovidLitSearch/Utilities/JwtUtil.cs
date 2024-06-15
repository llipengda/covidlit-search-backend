using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using Microsoft.IdentityModel.Tokens;

namespace CovidLitSearch.Utilities;

public static class JwtUtil
{
    public static string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AppSettings.Jwt.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = AppSettings.Jwt.Issuer,
            Audience = AppSettings.Jwt.Audience,
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Role, Enum.GetName(user.Role)!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                ]
            ),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static int GetUserId(ClaimsPrincipal claims)
    {
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(id!);
    }
}
