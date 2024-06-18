using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/codes")]
public class CodeController(ICodeService service) : ControllerBase
{
    [HttpPost("send")]
    public ActionResult SendCode([Required] string email)
    {
        return service.Send(email).Match<ActionResult>(Ok, BadRequest);
    }

    [HttpPost("verify")]
    public ActionResult<bool> VerifyCode([Required] string email, [Required] int code)
    {
        return Ok(service.Verify(email, code));
    }
}
