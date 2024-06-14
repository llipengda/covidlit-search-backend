using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class ArticleService(DbprojectContext context) : IArticleService
{
    public async Task<List<ArticleDTO>> GetArticles(int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        var data = await context
            .Database.SqlQuery<ArticleDTO>(
                $"""
                SELECT article.*, publish.*, write.author_name AS author
                FROM article
                LEFT JOIN publish on article.id = publish.article_id
                LEFT JOIN write on article.id = write.article_id
                WHERE article.url IS NOT NULL
                LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
                """
            )
            .AsNoTracking()
            .ToListAsync();
        return data.GroupBy(a => a.Id)
            .Select(g =>
            {
                var article = g.First();
                article.Authors = g.Select(a => a.Author!).ToList();
                if (article.Authors.All(a => a is null))
                {
                    article.Authors = [];
                }
                return article;
            })
            .ToList();
    }

    public async Task<Article?> GetArticleById(string id)
    {
        var data = await context
            .Database.SqlQuery<ArticleDTO>(
                $"""
                SELECT article.*, publish.*, write.author_name AS author
                FROM article
                LEFT JOIN publish on article.id = publish.article_id
                LEFT JOIN write on article.id = write.article_id
                WHERE id = {id}
                """
            )
            .AsNoTracking()
            .ToListAsync();

        return data.GroupBy(a => a.Id)
            .Select(g =>
            {
                var article = g.First();
                article.Authors = g.Select(a => a.Author!).ToList();
                if (article.Authors.All(a => a is null))
                {
                    article.Authors = [];
                }
                return article;
            })
            .FirstOrDefault();
    }
}
