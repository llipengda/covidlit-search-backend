using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/codes")]
public class CodeController(ICodeService service) : ControllerBase
{
    /// <summary>
    ///  Send a verification code to the email
    /// </summary>
    /// <param name="email"></param>
    /// <response code="400">Bad Request</response>
    /// <returns></returns>
    [HttpPost]
    public ActionResult SendCode([Required] string email)
    {
        return service.Send(email).Match<ActionResult>(Ok, BadRequest);
    }
}
