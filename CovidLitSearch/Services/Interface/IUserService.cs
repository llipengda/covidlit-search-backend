using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Services.Interface;

public interface IUserService
{
    Task<Result<LoginDto, Error>> Login(string email, string password);

    Task<Result<UserDto, Error>> Update(int id, UserDto userDto);
    
    Task<Result<UserDto, Error>> UpdatePassword(int? id, string? email, int? code, string? oldPwd, string newPwd);

    Task<Result<UserDto, Error>> Signup(string email, string password, int code);
}
