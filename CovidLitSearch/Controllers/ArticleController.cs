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
    public async Task<ActionResult<List<ArticleDTO>>> GetArticles(
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
    public async Task<ActionResult<ArticleDTO?>> GetArticleById(string id)
    {
        var article = await service.GetArticleById(id);
        if (article is null)
        {
            return NotFound();
        }
        return Ok(article);
    }
}
