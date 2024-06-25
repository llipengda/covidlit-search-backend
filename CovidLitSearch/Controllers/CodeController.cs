using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/codes")]
public class CodeController(ICodeService service) : ControllerBase
{
    [HttpPost]
    public ActionResult SendCode([Required] string email)
    {
        return service.Send(email).Match<ActionResult>(Ok, BadRequest);
    }
}
