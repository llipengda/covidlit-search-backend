using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CovidLitSearch.Services;

public class UserService(DbprojectContext context, ICodeService codeService, IMapper mapper)
    : IUserService
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
            .SingleOrDefaultAsync();

        if (user is null || !PasswordUtil.Verify(password, user.Salt, user.Password))
        {
            return new Error(ErrorCode.InvalidCredentials);
        }

        var token = JwtUtil.GenerateToken(user);

        var loginDto = mapper.Map<LoginDto>(user);

        loginDto.Token = token;

        return loginDto;
    }

    public async Task<Result<UserDto, Error>> Signup(string email, string password, int code)
    {
        if (!codeService.Verify(email, code).Unwrap())
        {
            return new Error(ErrorCode.InvalidVerificationCode);
        }

        if (!MailAddress.TryCreate(email, out _))
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
            .SingleOrDefaultAsync();

        if (data is not null && data.Email == email)
        {
            return new Error(ErrorCode.EmailAlreadyExists);
        }

        var nickName = email.Split('@')[0];

        var salt = PasswordUtil.GenerateSalt();

        password = PasswordUtil.Hash(password, salt);

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
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .SingleAsync();
    }

    public async Task<Result<UserDto, Error>> Update(int id, UserDto userDto)
    {
        var init = """UPDATE "user" SET""";
        var type = typeof(UserDto);
        var properties = type.GetProperties();
        var parameters = new List<NpgsqlParameter>();
        foreach (var v in properties)
        {
            var val = v.GetValue(userDto);
            var column = v.GetCustomAttribute<ColumnAttribute>();
            if (val is null || column is null) continue;
            init += $" {column.Name} = @{column.Name},";
            parameters.Add(new(column.Name, val));
        }

        init = init[..^1];
        init += " WHERE id = @id";
        parameters.Add(new("id", id));

        if (parameters.Count != 0)
        {
            await context.Database.ExecuteSqlRawAsync(init, parameters.ToArray());
        }

        var data = await context
            .Database.SqlQuery<User>(
                $"""
                 SELECT * FROM "user" WHERE id = {id}
                 """
            )
            .AsNoTracking()
            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        return data is not null ? data : new Error(ErrorCode.NotFound);
    }

    public async Task<Result<UserDto, Error>> UpdatePassword(int? id, string? email, int? code, string? oldPwd, string newPwd)
    {

       var user = await context.Database.SqlQuery<User>(
                $"""
                 SELECT * FROM "user" WHERE id = {id} OR email = {email}
                 """)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return new Error(ErrorCode.UserNotFound);
        }

        if (code is not null)
        {
            if (!codeService.Verify(user.Email, (int)code).Unwrap())
            {
                return new Error(ErrorCode.InvalidVerificationCode);
            }
        }
        else if (!PasswordUtil.Verify(oldPwd!, user.Salt, user.Password))
        {
            return new Error(ErrorCode.InvalidCredentials);
        }

        user.Salt = PasswordUtil.GenerateSalt();
        user.Password = PasswordUtil.Hash(newPwd, user.Salt);

        await context.Database.ExecuteSqlAsync(
            $"""
             UPDATE "user" SET password = {user.Password}, salt = {user.Salt} WHERE id = {id}
             """
        );

        return mapper.Map<UserDto>(user);
    }
}