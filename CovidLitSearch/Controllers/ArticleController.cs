using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidLitSearch.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ArticleController(IArticleService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Article>>> GetArticles()
    {
        var articles = await service.GetArticles();
        return Ok(articles);
    }
}
