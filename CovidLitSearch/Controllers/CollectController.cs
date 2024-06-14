using CovidLitSearch.Models;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/collect")]
public class CollectController(ICollectService service) : ControllerBase
{
    
    /// <summary>
    ///  Collect an article
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="articleId"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Collect>> Collect(
        [FromQuery] int userId,
        [FromQuery] string articleId
    )
    {
        return (await service.Collect(userId, articleId)).Match<ActionResult<Collect>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.AlreadyCollected => Conflict(error),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
        
    }
    
}