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
        
        var requireUrl = !allowNoUrl ? "AND article.url IS NOT NULL" : "";
        var searchQuery =  searchBy switch
        {
            ArticleSearchBy.Title => $"""
                                     SELECT
                                     	article.*,
                                     	publish.* 
                                     FROM
                                     	article
                                     	JOIN publish ON article.ID = publish.article_id 
                                     WHERE
                                     	( article.title LIKE @SEARCH ) {requireUrl}
                                     """,
            ArticleSearchBy.Author => $"""
                                      SELECT
                                      	article.*,
                                      	publish.* 
                                      FROM
                                      	article
                                      	JOIN publish ON article.ID = publish.article_id 
                                      WHERE 
                                          (article.authors LIKE @search) {requireUrl}
                                      """,
            ArticleSearchBy.Journal => $"""
                                       SELECT
                                       	article.*,
                                       	publish.* 
                                       FROM
                                       	article
                                       	JOIN publish ON article.ID = publish.article_id 
                                       WHERE (journal_name LIKE @search) {requireUrl}
                                       """,
            ArticleSearchBy.Title | ArticleSearchBy.Author
                => $"""
                   SELECT
                   	article.*,
                   	publish.* 
                   FROM
                   	article
                   	JOIN publish ON article.ID = publish.article_id 
                   WHERE
                   	article.title LIKE @search {requireUrl} UNION
                   SELECT
                   	article.*,
                   	publish.* 
                   FROM
                   	article
                   	JOIN publish ON article.ID = publish.article_id 
                   WHERE
                   	article.authors LIKE @search {requireUrl}
                   """,
            ArticleSearchBy.Author | ArticleSearchBy.Journal
                => $"""
                    SELECT
                    	article.*,
                    	publish.* 
                    FROM
                    	article
                    	JOIN publish ON article.ID = publish.article_id 
                    WHERE
                    	journal_name LIKE @search {requireUrl} UNION
                    SELECT
                    	article.*,
                    	publish.* 
                    FROM
                    	article
                    	JOIN publish ON article.ID = publish.article_id 
                    WHERE
                    	article.authors LIKE @search {requireUrl}
                    """,
            ArticleSearchBy.Title | ArticleSearchBy.Journal
                => $"""
                   SELECT
                   	article.*,
                   	publish.* 
                   FROM
                   	article
                   	JOIN publish ON article.ID = publish.article_id 
                   WHERE
                   	journal_name LIKE @search {requireUrl} UNION 
                   SELECT
                   	article.*,
                   	publish.* 
                   FROM
                   	article
                   	JOIN publish ON article.ID = publish.article_id 
                   WHERE
                   	article.title LIKE @search {requireUrl}
                   """,
            ArticleSearchBy.Author | ArticleSearchBy.Journal | ArticleSearchBy.Title
                => $"""
                   SELECT
                   	article.*,
                   	publish.* 
                   FROM
                   	article
                   	JOIN publish ON article.ID = publish.article_id 
                   WHERE
                   	journal_name LIKE @search {requireUrl} UNION
                   SELECT
                   	article.*,
                   	publish.* 
                   FROM
                   	article
                   	JOIN publish ON article.ID = publish.article_id 
                   WHERE
                   	article.authors LIKE @search {requireUrl} UNION
                   SELECT
                   	article.*,
                   	publish.* 
                   FROM
                   	article
                   	JOIN publish ON article.ID = publish.article_id 
                   WHERE
                   	article.title LIKE @search {requireUrl}
                   """,
            _ => ""
        };
        if (!allowNoUrl)
        {
            if(string.IsNullOrEmpty(searchQuery)){
                searchQuery = """
                              SELECT
                              	article.*,
                              	publish.* 
                              FROM
                              	article
                              	JOIN publish ON article.ID = publish.article_id 
                              WHERE article.url IS NOT NULL;
                              """;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery += " AND COALESCE(article.url, '111') <> '111'";
            }
            else
            {
                searchQuery = "WHERE COALESCE(article.url, '111') <> '111'";
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

        var countQuery = $"""
                          SELECT COUNT(*) AS "count"
                          FROM article
                          JOIN publish on article.id = publish.article_id
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
            .FirstOrDefaultAsync();

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
        string? focus,
        string? orderBy,
        bool? desc
    )
    {
        var orderQuery = "";
        if (orderBy is not null)
        {
            orderQuery = $" ORDER BY {orderBy}";
            if (desc is true)
            {
                orderQuery += " DESC";
            }
        }
        else
        {
            orderQuery = " ORDER BY publish_time DESC";
        }
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
                 {orderQuery}
                 LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data;
    }

    public async Task<Result<List<CiteDto>, Error>> GetCites(string id)
    {
        var data = await context
            .Database.SqlQuery<CiteDto>(
                $"""
                 WITH RECURSIVE c AS (
                   SELECT citing_id, cited_id, 0 AS flag
                   FROM cite WHERE citing_id = {id}
                   UNION ALL
                   SELECT cite.citing_id, cite.cited_id, 1 AS flag
                   FROM c
                   JOIN cite ON c.cited_id = cite.citing_id
                 )
                 SELECT cited_id AS "id", NULL AS "title", flag FROM c
                 """
            )
            .AsNoTracking()
            .ToListAsync();
            
            data.ForEach(c => c.Title = GetArticleById(c.Id, null).Result.Unwrap().Title);

            return data;
    }
}