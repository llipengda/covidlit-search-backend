using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.AspNetCore.Authorization;
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
        [Required] string email,
        [Required] string password
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
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpPost("signup")]
    public async Task<ActionResult<UserDto>> Signup(
        [Required] string email,
        [Required] string password,
        [Required] int code
    )
    {
        return (await service.Signup(email, password, code)).Match<ActionResult<UserDto>>(
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
    /// <param name="userDto"></param>
    /// <returns></returns>
    [HttpPut("update")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Update([FromBody][Required] UserDto userDto)
    {
        return (await service.Update(User.GetId(), userDto)).Match<ActionResult<UserDto>>(
            res => Ok(res),
            _ => NoContent()
        );
    }

    /// <summary>
    /// Update a user's password 
    /// either by code or old password
    /// </summary>
    /// <param name="code"></param>
    /// <param name="oldPwd"></param>
    /// <param name="newPwd"></param>
    /// <returns></returns>
    [HttpPut("update-password")]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdatePassword(
        int? code,
        string? oldPwd,
        [Required] string newPwd
    )
    {
        if (code is null && oldPwd is not null)
        {
            return BadRequest("Old password or code is required");
        }
        
        return (await service.UpdatePassword(User.GetId(), code, oldPwd, newPwd)).Match<ActionResult<UserDto>>(
            res => Ok(res),
            error => Unauthorized(error)
        );
    }
}
