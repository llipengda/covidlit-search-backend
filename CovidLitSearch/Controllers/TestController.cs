using CovidLitSearch.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet("no-auth")]
    public IActionResult NoAuth()
    {
        return Ok();
    }

    [HttpGet("auth")]
    [Authorize]
    public IActionResult Auth()
    {
        return Ok();
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        return Ok();
    }

    [HttpGet("userid")]
    [Authorize]
    public ActionResult<int> UserId()
    {
        return Ok(User.GetId());
    }
}
