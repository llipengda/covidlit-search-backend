using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CovidLitSearch.Services;

public class ArticleService(DbprojectContext context) : IArticleService
{
    public async Task<List<ArticleDTO>> GetArticles(
        int page,
        int pageSize,
        bool allowNoUrl,
        string? search,
        ArticleSearchBy? searchBy
    )
    {
        page = page <= 0 ? 1 : page;
        var searchQuery = searchBy switch
        {
            ArticleSearchBy.Title => "WHERE article.title LIKE @search",
            ArticleSearchBy.Author => "WHERE article.authors LIKE @search",
            ArticleSearchBy.Journal => "WHERE article.journal LIKE @search",
            ArticleSearchBy.Title | ArticleSearchBy.Author
                => "WHERE article.title LIKE @search OR article.authors LIKE @search",
            _ => ""
        };
        if (!allowNoUrl)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery += " AND article.url IS NOT NULL";
            }
            else
            {
                searchQuery = "WHERE article.url IS NOT NULL";
            }
        }
        var query = $"""
            SELECT article.*, publish.*
            FROM article
            LEFT JOIN publish on article.id = publish.article_id
            {searchQuery}
            LIMIT @pageSize OFFSET @offset
            """;
        var parameters = new List<NpgsqlParameter>
        {
            new("search", $"%{search}%"),
            new("pageSize", pageSize),
            new("offset", (page - 1) * pageSize)
        };
        return await context
            .Database.SqlQueryRaw<ArticleDTO>(query, parameters.ToArray())
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ArticleDTO?> GetArticleById(string id)
    {
        return await context
            .Database.SqlQuery<ArticleDTO>(
                $"""
                SELECT article.*, publish.*
                FROM article
                LEFT JOIN publish on article.id = publish.article_id
                WHERE id = {id}
                """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
}
