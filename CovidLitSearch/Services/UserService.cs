using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CovidLitSearch.Services;

public class UserService(DbprojectContext context, IConfiguration configuration) : IUserService
{
    public async Task<Result<LoginDTO, Error>> Login(string email, string password)
    {
        var user = await context
            .Database.SqlQuery<User>(
                $"""
                SELECT * FROM "user" WHERE email = {email}
                """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null || user.Password != password)
        {
            return new Error(ErrorCode.InvalidCredentials);
        }

        var token = GenerateJwtToken(user);

        return new LoginDTO { Email = email, Token = token };
    }

    public async Task<Result<User?, Error>> Signup(string email, string password)
    {
        if (!MailAddress.TryCreate(email, out var _))
        {
            return new Error(ErrorCode.InvalidEmail);
        }

        var data = await context
            .Database.SqlQuery<User>(
                $"""
                SELECT * FROM "user" WHERE email = {email}
                """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (data != null && data.Email == email)
        {
            return new Error(ErrorCode.EmailAlreadyExists);
        }

        var nickName = email.Split('@')[0];

        await context.Database.ExecuteSqlAsync(
            $"""
            INSERT INTO "user" ("email", "password", "nickname") VALUES ({email}, {password}, {nickName})
            """
        );

        return await context
            .Database.SqlQuery<User>(
                $"""
                SELECT * FROM "user" WHERE email = {email}
                """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Subject = new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Role, Enum.GetName(user.Role)!) }
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
}
