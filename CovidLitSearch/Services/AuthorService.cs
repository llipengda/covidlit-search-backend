using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class AuthorService(DbprojectContext context) : IAuthorService
{
    public async Task<Result<List<Author>, Error>> GetAuthors(
        string? search,
        int page,
        int pageSize
    )
    {
        page = page <= 0 ? 1 : page;
        var data = await context
            .Database.SqlQuery<Author>(
                $"""
                 SELECT * FROM "author" WHERE "name" LIKE '%'|| {search} || '%' 
                 order by "name"
                 LIMIT {pageSize} OFFSET {(
                    page - 1
                ) * pageSize}
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data;
    }

    public async Task<Result<Author?, Error>> GetAuthorById(string name)
    {
        var data = await context.Database.SqlQuery<Author>(
            $"""
             SELECT * FROM "author" WHERE "name" = {name}
             """
        ).AsNoTracking().SingleOrDefaultAsync();

        if (data is null)
        {
            return new Error(ErrorCode.NotFound);
        }

        return data;
    }

    public async Task<Result<int, Error>> GetAuthorsCount(string? search)
    {
        var count = await context.Database.SqlQuery<CountType>(
            $"""
             SELECT COUNT(*) FROM "author" WHERE "name" LIKE '%' || {search} || '%'
             """
        ).AsNoTracking().SingleOrDefaultAsync();

        return new Result<int, Error>(count!.Count);
    }

    public async Task<Result<List<ArticleDto>, Error>> GetArticlesByAuthor(string name, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        var data = await context.Database.SqlQuery<Author>(
            $"""
             SELECT * FROM "author" WHERE "name" = {name}
             """
        ).AsNoTracking().SingleOrDefaultAsync();

        if (data is null)
        {
            return new Error(ErrorCode.NotFound);
        }
        
        var articles = await context.Database.SqlQuery<ArticleDto>(
            $"""
             SELECT
               article.*,
               publish.*
             FROM
               write
               JOIN article ON write.article_id = article."id"
               JOIN publish ON publish.article_id = write.article_id
             WHERE
               write."author_name" = {name}
             ORDER BY
               publish_time DESC
             LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
             """
        ).AsNoTracking().ToListAsync();
        
        return articles;
    }
}