using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
    
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult Swagger()
    {
        return Redirect("/swagger");
    }
}