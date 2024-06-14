using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
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
        var login = await service.Login(email, password);
        return login.Token != null ? Ok(login) : Unauthorized(login);
    }

    [HttpPost("signup")]
    public async Task<ActionResult<User>> Signup(
        [FromQuery] string email,
        [FromQuery] string password
    )
    {
        var signup = await service.Signup(email, password);
        return signup != null ? Ok(signup) : Ok();
    }
    
}