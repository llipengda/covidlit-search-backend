using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/codes")]
public class VerifyCodeController(IVerifyCodeService service) : ControllerBase
{
    [HttpPost("send")]
    public ActionResult SendCode(string email)
    {
        return service.Send(email).Match<ActionResult>(Ok, BadRequest);
    }

    [HttpPost("verify")]
    public ActionResult<bool> VerifyCode(string email, int code)
    {
        return Ok(service.Verify(email, code));
    }
}
