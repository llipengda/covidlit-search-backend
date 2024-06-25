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
    /// <response code="401">Unauthorized</response>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginDto>> Login(
        [Required][FromForm] string email,
        [Required][FromForm] string password
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
    /// <response code="400">Bad Request, InvalidEmail or InvalidVerificationCode</response>
    /// <response code="409">Conflict</response>
    /// <returns></returns>
    [HttpPost("signup")]
    public async Task<ActionResult<UserDto>> Signup(
        [Required][FromForm] string email,
        [Required][FromForm] string password,
        [Required][FromForm] int code
    )
    {
        return (await service.Signup(email, password, code)).Match<ActionResult<UserDto>>(
            res => Ok(res),
            error =>
                error.Code switch
                {
                    ErrorCode.EmailAlreadyExists => Conflict(error),
                    ErrorCode.InvalidEmail => BadRequest(error),
                    ErrorCode.InvalidVerificationCode => BadRequest(error),
                    _ => throw new Exception(error.ToString())
                }
        );
    }

    /// <summary>
    ///  Update a user
    /// </summary>
    /// <param name="userDto"></param>
    /// <response code="204">No Content</response>
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
    /// <param name="email"></param>
    /// <param name="oldPwd"></param>
    /// <param name="newPwd"></param>
    /// <response code="400">Bad Request, Old password or code is required</response>
    /// <response code="401">Unauthorized</response>
    /// <returns></returns>
    [HttpPut("update-password")]
    public async Task<ActionResult<UserDto>> UpdatePassword(
        [FromForm] int? code,
        [FromForm] string? email,
        [FromForm] string? oldPwd,
        [Required][FromForm] string newPwd
    )
    {
        if (code is null && oldPwd is not null)
        {
            return BadRequest("Old password or code is required");
        }
        
        return (await service.UpdatePassword(User.TryGetId(), email, code, oldPwd, newPwd)).Match<ActionResult<UserDto>>(
            res => Ok(res),
            error => Unauthorized(error)
        );
    }
}
