using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
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
    /// Get a single article by id
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>Article</returns>
    /// <response code="200">Success</response>
    /// <response code="404">Not Found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto?>> GetArticleById(string id)
    {
        var article = await service.GetArticleById(id);
        if (article is null)
        {
            return NotFound();
        }
        return Ok(article);
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
        var articles = await service.GetArticlesByResearch(page, pageSize, studyType, addressedPopulation, challenge, focus);
        return Ok(articles);
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
        var cites = await service.GetCites(page, pageSize, id);
        return Ok(cites);
    }
    
}
