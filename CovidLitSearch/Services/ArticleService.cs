using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class ArticleService(DbprojectContext context) : IArticleService
{
    public async Task<List<Article>> GetArticles()
    {
        return await context
            .Set<Article>()
            .FromSqlRaw("SELECT * FROM article LIMIT 100")
            .ToListAsync();
    }
}
