using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class CollectService(DbprojectContext context) : ICollectService
{
    public async Task<Result<Collect, Error>> Collect(int userId, string articleId)
    {
        var collect = await context
            .Database.SqlQuery<Collect>(
                $"""
                 SELECT * FROM "collect" WHERE user_id = {userId} AND article_id = {articleId}
                 """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (collect is not null)
        {
            return new Error(ErrorCode.AlreadyCollected);
        }

        await context.Database.ExecuteSqlAsync(
            $"""
             INSERT INTO "collect" VALUES({userId}, {articleId});
             """
        );

        return await context
            .Database.SqlQuery<Collect>(
                $"""
                 SELECT * FROM "collect" WHERE user_id = {userId} AND article_id = {articleId}
                 """
            )
            .AsNoTracking()
            .SingleAsync();
    }

    public async Task<Result<List<CollectDto>, Error>> GetCollects(
        int page,
        int pageSize,
        int userId
    )
    {
        page = page < 1 ? 1 : page;
        var data = await context
            .Database.SqlQuery<CollectDto>(
                $"""
                 SELECT
                   "collect".*,
                   title,
                   authors,
                   abstract,
                   journal_name 
                 FROM
                   "collect"
                   JOIN article ON "collect".article_id = article."id"
                   JOIN publish ON publish.article_id = "collect".article_id 
                 WHERE
                   "user_id" = {userId}
                 ORDER BY "title"
                 LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data;
    }

    public async Task<Result<Error>> DeleteCollect(int userId, string articleId)
    {
        var collect = await context
            .Database.SqlQuery<Collect>(
                $"""
                 SELECT * FROM "collect" WHERE user_id = {userId} AND article_id = {articleId}
                 """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (collect is null)
        {
            return new Error(ErrorCode.NoSuchElement);
        }

        await context.Database.ExecuteSqlAsync(
            $"""
             DELETE FROM "collect" WHERE user_id = {userId} AND article_id = {articleId};
             """
        );

        return new();
    }

    public async Task<Result<bool, Error>> IsCollected(int userId, string articleId)
    {
        var data = await context
            .Database.SqlQuery<Collect>(
                $"""
                 SELECT * FROM "collect" WHERE user_id = {userId} AND article_id = {articleId}
                 """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();
        return new Result<bool, Error>(data is not null);
    }

    public async Task<Result<int, Error>> GetCollectsCount(int userId)
    {
        var count = await context
            .Database.SqlQuery<CountType>(
                $"""
                 SELECT COUNT(*) FROM "collect" WHERE "user_id" = {userId}
                 """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();
        return new Result<int, Error>(count!.Count);
    }
}