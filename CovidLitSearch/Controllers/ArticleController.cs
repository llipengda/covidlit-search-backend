using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticleController(IArticleService service) : ControllerBase
{
    /// <summary>
    /// Search articles
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="allowNoUrl"></param>
    /// <param name="search"></param>
    /// <param name="searchBy"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ArticleDto>>> GetArticles(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] bool allowNoUrl = false,
        [FromQuery] string? search = null,
        [FromQuery] ArticleSearchBy? searchBy = null
    )
    {
        var articles = await service.GetArticles(page, pageSize, allowNoUrl, search, searchBy);
        return Ok(articles);
    }

    /// <summary>
    ///  Get article by id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto?>> GetArticleById(
        [FromRoute] string id,
        [FromQuery] int userId
        )
    {
        return (await service.GetArticleById(id, userId)).Match<ActionResult<ArticleDto?>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.NoData => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
    /// <summary>
    /// Get articles by research type, addressed population, challenge, and focus
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="studyType"></param>
    /// <param name="addressedPopulation"></param>
    /// <param name="challenge"></param>
    /// <param name="focus"></param>
    /// <returns></returns>
    [HttpGet("research")]
    public async Task<ActionResult<List<ArticleDto>>> GetArticlesByResearch(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] string? studyType = null,
        [FromQuery] string? addressedPopulation = null,
        [FromQuery] string? challenge = null,
        [FromQuery] string? focus = null
    )
    {
        return (await service.GetArticlesByResearch(page, pageSize, studyType, addressedPopulation, challenge, focus)).Match<ActionResult<List<ArticleDto>>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.NoData => NoContent(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
    /// <summary>
    ///  Get cites by article id
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("cite/{id}")]
    public async Task<ActionResult<List<CiteDto>>> GetCites(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromRoute] string id
    )
    {
        return (await service.GetCites(page, pageSize, id)).Match<ActionResult<List<CiteDto>>>(
            res => Ok(res),
            error => error.Code switch
            {
                ErrorCode.NoData => NoContent(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }
        );
    }
    
}
