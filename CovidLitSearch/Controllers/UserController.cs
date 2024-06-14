using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService service) : ControllerBase
{
    /// <summary>
    ///  Login a user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginDto>> Login(
        [FromQuery] string email,
        [FromQuery] string password
    )
    {
        return (await service.Login(email, password)).Match<ActionResult<LoginDto>>(
            res => Ok(res),
            error => Unauthorized(error)
        );
    }

    /// <summary>
    ///   Signup a new user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost("signup")]
    public async Task<ActionResult<User>> Signup(
        [FromQuery] string email,
        [FromQuery] string password
    )
    {
        return (await service.Signup(email, password)).Match<ActionResult<User>>(
            res => Ok(res),
            error =>
                error.Code switch
                {
                    ErrorCode.EmailAlreadyExists => Conflict(error),
                    ErrorCode.InvalidEmail => BadRequest(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError)
                }
        );
    }
    
    [HttpPut("update")]
    public async Task<ActionResult<User>> Update(
        [FromQuery] int id,
        [FromBody] UserDto userDto
    )
    {
        return (await service.Update(id, userDto)).Match<ActionResult<User>>(
            res => Ok(res),
            error => StatusCode(StatusCodes.Status500InternalServerError)
        );
    }
    
    
}
