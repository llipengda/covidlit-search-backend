using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.Enums;
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <response code="500"></response>
    /// <exception cref="Exception"></exception>
    [HttpGet("exception")]
    public IActionResult Exception()
    {
        throw new Exception("Test exception");
    }

    [HttpGet("error")]
    public ActionResult<Error> Error([Required] ErrorCode code)
    {
        return new Error(code);
    }

    [HttpGet("userid")]
    [Authorize]
    public ActionResult<int> UserId()
    {
        return Ok(User.GetId());
    }
}
