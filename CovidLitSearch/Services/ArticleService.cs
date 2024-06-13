using System.Diagnostics;
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
        var sw = new Stopwatch();
        sw.Start();
        var data = await context
            .Database.SqlQuery<ArticleDTO>(
                $"""
                SELECT article.*, publish.*, write.author_name AS author
                FROM article
                LEFT JOIN publish on article.id = publish.article_id
                    AND article.url IS NOT NULL
                LEFT JOIN write on article.id = write.article_id
                LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
                """
            )
            .AsNoTracking()
            .ToListAsync();
        var time1 = sw.ElapsedMilliseconds;
        await Console.Out.WriteLineAsync($"{time1}");
        var res = data.GroupBy(a => a.Id)
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
        var time2 = sw.ElapsedMilliseconds;
        await Console.Out.WriteLineAsync($"{time2 - time1}");
        return res;
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
