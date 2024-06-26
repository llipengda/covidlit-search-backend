using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CovidLitSearch.Services;

public class ArticleService(DbprojectContext context) : IArticleService
{
    public async Task<Result<List<ArticleDto>, Error>> GetArticles(
        int page,
        int pageSize,
        bool allowNoUrl,
        string? search,
        ArticleSearchBy? searchBy,
        string? orderBy,
        bool? desc
    )
    {
        page = page <= 0 ? 1 : page;
        
        var searchQuery =  searchBy switch
        {
            ArticleSearchBy.Title => "WHERE (article.title LIKE @search)",
            ArticleSearchBy.Author => "WHERE (article.authors LIKE @search)",
            ArticleSearchBy.Journal => "WHERE (journal_name LIKE @search)",
            ArticleSearchBy.Title | ArticleSearchBy.Author
                => "WHERE (article.title LIKE @search OR article.authors LIKE @search)",
            ArticleSearchBy.Author | ArticleSearchBy.Journal
                => "WHERE (article.authors LIKE @search OR journal_name LIKE @search)",
            ArticleSearchBy.Title | ArticleSearchBy.Journal
                => "WHERE (article.title LIKE @search OR journal_name LIKE @search)",
            ArticleSearchBy.Author | ArticleSearchBy.Journal | ArticleSearchBy.Title
                => "WHERE (article.title LIKE @search OR article.authors LIKE @search OR journal_name LIKE @search)",
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

        if (orderBy is not null)
        {
            searchQuery += $" ORDER BY {orderBy}";
            if (desc is true)
            {
                searchQuery += " DESC";
            }
        }
        else
        {
            searchQuery += " ORDER BY publish_time DESC";
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
            .Database.SqlQueryRaw<ArticleDto>(query, parameters.ToArray())
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<Result<int, Error>> GetArticlesCount(
        bool allowNoUrl,
        string? search,
        ArticleSearchBy? searchBy
    )
    {
        var searchQuery = searchBy switch
        {
            ArticleSearchBy.Title => "WHERE article.title LIKE @search",
            ArticleSearchBy.Author => "WHERE article.authors LIKE @search",
            ArticleSearchBy.Journal => "WHERE article.journal LIKE @search",
            ArticleSearchBy.Title | ArticleSearchBy.Author
                => "WHERE article.title LIKE @search OR article.authors LIKE @search",
            ArticleSearchBy.Author | ArticleSearchBy.Journal
                => "WHERE article.authors LIKE @search OR article.journal LIKE @search",
            ArticleSearchBy.Title | ArticleSearchBy.Journal
                => "WHERE article.title LIKE @search OR article.journal LIKE @search",
            ArticleSearchBy.Author | ArticleSearchBy.Journal | ArticleSearchBy.Title
                => "WHERE article.title LIKE @search OR article.authors LIKE @search OR article.journal LIKE @search",
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

        var countQuery = $"""
                          SELECT COUNT(*) AS "count"
                          FROM article
                          {searchQuery}
                          """;

        var parameters = new List<NpgsqlParameter>
        {
            new("search", $"%{search}%")
        };

        var totalCount = await context
            .Database.SqlQueryRaw<CountType>(countQuery, parameters.ToArray())
            .AsNoTracking()
            .SingleAsync();

        return new Result<int, Error>(totalCount.Count);
    }


    public async Task<Result<ArticleDto, Error>> GetArticleById(string id, int? userId)
    {
        var article = await context
            .Database.SqlQuery<ArticleDto>(
                $"""
                 SELECT article.*, publish.*
                 FROM article
                 LEFT JOIN publish on article.id = publish.article_id
                 WHERE id = {id}
                 """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (article is null)
        {
            return new Error(ErrorCode.NotFound);
        }

        if (userId is not null)
        {
            await context.Database.ExecuteSqlAsync(
                $"INSERT INTO history(user_id, article_id, time) VALUES ({userId}, {id}, now()) "
            );
        }

        return article;
    }

    public async Task<Result<List<ArticleDto>, Error>> GetArticlesByResearch(
        int page,
        int pageSize,
        string? studyType,
        string? addressedPopulation,
        string? challenge,
        string? focus
    )
    {
        var data = await context
            .Database.SqlQuery<ArticleDto>(
                $"""
                 SELECT "id", "title", "abstract", "doi", "license", 
                   "publish_time", "url", "study_type", "addressed_population",
                   "challenge", "focus", "authors", "journal_name", "volume", "pages"
                 FROM (
                   SELECT * 
                   FROM "article"
                   WHERE "study_type" LIKE '%' || {studyType} || '%'
                   OR "addressed_population" LIKE '%' || {addressedPopulation} || '%'
                   OR "challenge" LIKE '%' || {challenge} || '%'
                   OR "focus" LIKE '%' || {focus} || '%'
                 ) AS art
                 JOIN publish ON "id" = publish."article_id"
                 JOIN "write" ON "id" = "write"."article_id"
                 LIMIT {pageSize} OFFSET {(page - 1) * pageSize} 
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data;
    }

    public async Task<Result<List<CiteDto>, Error>> GetCites(int page, int pageSize, string id)
    {
        page = page <= 0 ? 1 : page;
        var data = await context
            .Database.SqlQuery<CiteDto>(
                $"""
                 WITH RECURSIVE c AS (
                   SELECT citing_id, cited_id
                   FROM cite WHERE citing_id = {id}
                   UNION ALL
                   SELECT cite.citing_id, cite.cited_id
                   FROM c
                   JOIN cite ON c.cited_id = cite.citing_id
                 )
                 SELECT cited_id AS "id", title FROM c JOIN article ON cited_id = "id" LIMIT {pageSize} OFFSET {(
                     page - 1
                 ) * pageSize}
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data;
    }
}