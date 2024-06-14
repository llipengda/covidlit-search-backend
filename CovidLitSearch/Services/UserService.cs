using System.Net.Mail;
using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class UserService(DbprojectContext context) : IUserService
{
    public async Task<Result<LoginDTO, Error>> Login(string email, string password)
    {
        var data = await context
            .Database.SqlQuery<User>(
                $"""
                SELECT * FROM "user" WHERE email = {email}
                """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (data == null || data.Password != password)
        {
            return new Error(ErrorCode.InvalidCredentials);
        }

        var token = "token";

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
}
