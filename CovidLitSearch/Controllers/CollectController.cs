using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
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
    
    /// <summary>
    ///  Get collection list
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<CollectDto>>> GetCollects(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] int userId
    )
    {
        return (await service.GetCollects(page, pageSize, userId)).Match<ActionResult<List<CollectDto>>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.NoData => NoContent(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
    /// <summary>
    ///  Delete a collection
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="articleId"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ActionResult<Unit>> DeleteCollect(
        [FromQuery] int userId,
        [FromQuery] string articleId
    )
    {
        return (await service.DeleteCollect(userId, articleId)).Match<ActionResult<Unit>>(
            _ => NoContent(),
            error => error.Code switch
            {
                ErrorCode.InvalidCredentials => BadRequest(error),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
    
    
}