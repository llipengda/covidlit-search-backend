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
    [HttpPost("login")]
    public async Task<ActionResult<LoginDTO>> Login(
        [FromQuery] string email,
        [FromQuery] string password
    )
    {
        return (await service.Login(email, password)).Match<ActionResult<LoginDTO>>(
            res => Ok(res),
            error => Unauthorized(error)
        );
    }

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
}
