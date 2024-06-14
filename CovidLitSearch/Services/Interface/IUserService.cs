using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Services.Interface;

public interface IUserService
{
    Task<Result<LoginDTO, Error>> Login(string email, string password);

    Task<Result<User?, Error>> Signup(string email, string password, int code);
}
