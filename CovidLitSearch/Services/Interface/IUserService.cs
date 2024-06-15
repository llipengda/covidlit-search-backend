using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Services.Interface;

public interface IUserService
{
    Task<Result<NotAFile, Error>> Login(string email, string password);

    Task<Result<User?, Error>> Update(int id, UserDto userDto);
    
    Task<Result<User?, Error>> UpdatePassword(int id, string oldPwd, string newPwd);

    Task<Result<User?, Error>> Signup(string email, string password, int code);
}
