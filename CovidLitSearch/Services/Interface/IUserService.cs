using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Services.Interface;

public interface IUserService
{
    Task<LoginDTO> Login(string email, string password);

    Task<User?> Signup(string email, string password);
    
    
}