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
    
    /// <summary>
    ///  Update a user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userDto"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<ActionResult<User>> Update(
        [FromQuery] int id,
        [FromBody] UserDto userDto
    )
    {
        return (await service.Update(id, userDto)).Match<ActionResult<User>>(
            res => Ok(res),
            _ => NoContent()
        );
    }
    
    /// <summary>
    /// Update a user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="oldPwd"></param>
    /// <param name="newPwd"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult<User>> UpdatePassword(
        [FromQuery] int id,
        [FromQuery] string oldPwd,
        [FromQuery] string newPwd
    )
    {
        return (await service.UpdatePassword(id, oldPwd, newPwd)).Match<ActionResult<User>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.InvalidCredentials => Unauthorized(error),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );  
    }
    
}
