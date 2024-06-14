using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class UserService(DbprojectContext context) : IUserService
{
    public async Task<LoginDTO> Login(string email, string password)
    {
        var data = await context.Database.SqlQuery<User>(
                $"""
                 SELECT * FROM "user" WHERE email = {email}
                 """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();
            

        if (data == null || data.Password != password)
        {
            return new LoginDTO { Email = email, Token = null! };
        }

        var token = "token";

        return new LoginDTO { Email = email, Token = token };
    }

    public async Task<User?> Signup(string email, string password)
    {
        var data = await context.Database.SqlQuery<User>(
                $"""
                 SELECT * FROM "user" WHERE email = {email}
                 """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (data != null && data.Email == email)
        {
            throw new Exception("Email already exists");
        }

        await context.Database.ExecuteSqlAsync(
            $"""
             INSERT INTO "user" ("email", "password") VALUES ({email}, {password})
             """
        );
        
        return await context.Database.SqlQuery<User>(
                $"""
                 SELECT * FROM "user" WHERE email = {email}
                 """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    
}