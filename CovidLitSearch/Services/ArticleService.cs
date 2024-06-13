using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

    public async Task<Article?> GetArticleById(string id)
    {
        return await context
            .Set<Article>()
            .FromSqlInterpolated($"SELECT * FROM article WHERE id = {id}")
            .SingleOrDefaultAsync();
    }
}
