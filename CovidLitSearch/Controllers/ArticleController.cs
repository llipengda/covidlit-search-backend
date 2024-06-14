using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/article")]
public class ArticleController(IArticleService service) : ControllerBase
{
    [HttpGet("")]
    public async Task<ActionResult<List<ArticleDTO>>> GetArticles(
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var articles = await service.GetArticles(page, pageSize);
        return Ok(articles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Article?>> GetArticleById(string id)
    {
        var article = await service.GetArticleById(id);
        return Ok(article);
    }
}
