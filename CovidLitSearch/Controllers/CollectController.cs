using System.ComponentModel.DataAnnotations;
using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/collects")]
public class CollectController(ICollectService service) : ControllerBase
{
    /// <summary>
    ///  Collect an article
    /// </summary>
    /// <param name="articleId"></param>
    /// <reponse code="201">Created</reponse>
    /// <reponse code="409">Conflict</reponse>
    /// <returns></returns>
    [HttpPost("{articleId}")]
    [Authorize]
    public async Task<ActionResult<Collect>> Collect(string articleId)
    {
        return (await service.Collect(User.GetId(), articleId)).Match<ActionResult<Collect>>(
            res => Created($"/article/{res.ArticleId}", res),
            err => Conflict(err)
        );
    }

    /// <summary>
    ///  Get collection list
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<CollectDto>>> GetCollects(
        [Required] int page,
        [Required] int pageSize
    )
    {
        return (await service.GetCollects(page, pageSize, User.GetId())).Unwrap();
    }

    /// <summary>
    ///  Delete a collection
    /// </summary>
    /// <param name="articleId"></param>
    /// <returns></returns>
    /// <response code="204">Successfully deleted</response>
    /// <response code="400">Bad request</response>
    [HttpDelete("{articleId}")]
    [Authorize]
    public async Task<ActionResult> DeleteCollect(string articleId)
    {
        return (await service.DeleteCollect(User.GetId(), articleId)).Match<ActionResult>(
            NoContent, BadRequest
        );
    }
    
    /// <summary>
    ///  Check if an article is collected
    /// </summary>
    /// <param name="articleId"></param>
    /// <returns></returns>
    [HttpGet("is/{articleId}")]
    [Authorize]
    public async Task<ActionResult<bool>> IsCollected(string articleId)
    {
        return (await service.IsCollected(User.GetId(), articleId)).Unwrap();
    }
    
    /// <summary>
    ///  Get collection count
    /// </summary>
    /// <returns></returns>
    [HttpGet("count")]
    [Authorize]
    public async Task<ActionResult<int>> GetCollectsCount()
    {
        return (await service.GetCollectsCount(User.GetId())).Unwrap();
    }
    
}
