using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class CollectService(DbprojectContext context) : ICollectService
{
    public async Task<Result<Collect?, Error>> Collect(int userId, string articleId)
    {
        var collect = await context.Database.SqlQuery<Collect>(
                $"""
                 SELECT * FROM "collect" WHERE user_id = {userId} AND article_id = {articleId}
                 """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (collect is not null)
        {
            return new Error(ErrorCode.AlreadyCollected);
        }
        
        await context.Database.ExecuteSqlAsync(
            $"""
            INSERT INTO "collect" VALUES({userId}, {articleId});
            """
        );

        return await context.Database.SqlQuery<Collect>(
                $"""
                 SELECT * FROM "collect" WHERE user_id = {userId} AND article_id = {articleId}
                 """
            )
            .AsNoTracking()
            .SingleAsync();
    }
}