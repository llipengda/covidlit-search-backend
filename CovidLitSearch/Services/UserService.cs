using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Npgsql.Replication.PgOutput.Messages;

namespace CovidLitSearch.Services;

public class UserService(DbprojectContext context, IConfiguration configuration) : IUserService
{
    public async Task<Result<LoginDto, Error>> Login(string email, string password)
    {
        var user = await context
            .Database.SqlQuery<User>(
                $"""
                SELECT * FROM "user" WHERE email = {email}
                """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null || !PasswordUtil.VerifyPassword(password, user.Salt, user.Password))
        {
            return new Error(ErrorCode.InvalidCredentials);
        }

        var token = GenerateJwtToken(user);

        return new LoginDto { Email = email, Token = token };
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

        var salt = PasswordUtil.GenerateSalt();

        password = PasswordUtil.HashPassword(password, salt);

        await context.Database.ExecuteSqlAsync(
            $"""
            INSERT INTO "user" ("email", "password", "salt", "nickname") VALUES ({email}, {password}, {salt}, {nickName})
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

    public async Task<Result<User?, Error>> Update(int id, UserDto userDto)
    {
        var init = "UPDATE \"user\" SET";
        Type type = typeof(UserDto);
        var properties = type.GetProperties();
        var parameters = new List<NpgsqlParameter>();
        foreach (var v in properties)
        {
            var val = v.GetValue(userDto);
            var column = v.GetCustomAttribute<ColumnAttribute>();
            if (val is not null && column is not null)
            {
                init += $" {column.Name} = @{column.Name},";
                parameters.Add(new(column.Name, val));
            }
        }

        init = init[..^1];
        init += " WHERE id = @id";
        parameters.Add(new("id", id));

        if (parameters.Count != 0)
        {
            await context.Database.ExecuteSqlRawAsync(init, parameters.ToArray());
        }
        
        return await context
            .Database.SqlQuery<User>(
                $"""
                SELECT * FROM "user" WHERE id = {id}
                """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<Result<User?, Error>> UpdatePassword(int id, string oldPwd, string newPwd)
    {
        var user = await context.Database.SqlQuery<User>(
                $"""
                 SELECT * FROM "user" WHERE id = {id}
                 """)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null || !PasswordUtil.VerifyPassword(oldPwd, user.Salt, user.Password))
        {
            return new Error(ErrorCode.InvalidCredentials);
        }
        
        user.Salt = PasswordUtil.GenerateSalt();
        user.Password = PasswordUtil.HashPassword(newPwd, user.Salt);
        
        await context.Database.ExecuteSqlAsync(
            $"""
            UPDATE "user" SET password = {user.Password}, salt = {user.Salt} WHERE id = {id}
            """
        );

        return user;
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
