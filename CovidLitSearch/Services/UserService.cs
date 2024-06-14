using System.Net.Mail;
using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class UserService(DbprojectContext context, IVerifyCodeService verifyCodeService)
    : IUserService
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

        if (user is null || !PasswordUtil.VerifyPassword(password, user.Salt, user.Password))
        {
            return new Error(ErrorCode.InvalidCredentials);
        }

        var token = JwtUtil.GenerateToken(user);

        return new LoginDTO { Email = email, Token = token };
    }

    public async Task<Result<User?, Error>> Signup(string email, string password, int code)
    {
        if (!verifyCodeService.Verify(email, code))
        {
            return new Error(ErrorCode.InvalidVerifyCode);
        }
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
            INSERT INTO "user" ("email", "password", "salt", "nickname") 
            VALUES ({email}, {password}, {salt}, {nickName})
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
}
