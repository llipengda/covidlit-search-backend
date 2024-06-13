using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
public class ArticleController(IArticleService service) : ControllerBase
{
    [HttpGet("api/article")]
    public async Task<ActionResult<List<Article>>> GetArticles()
    {
        var articles = await service.GetArticles();
        return Ok(articles);
    }

    [HttpGet("api/article/{id}")]
    public async Task<ActionResult<Article?>> GetArticleById(string id)
    {
        var article = await service.GetArticleById(id);
        return Ok(article);
    }
}
