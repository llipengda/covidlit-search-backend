using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class SubscribeService(DbprojectContext context) : ISubscribeService
{
    public async Task<Result<Subscribe?, Error>> Subscribe(int userId, string journalName)
    {
        var collect = await context.Database.SqlQuery<Subscribe>(
                $"""
                 SELECT * FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName}
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
             INSERT INTO "subscribe" VALUES({userId}, {journalName});
             """
        );

        return await context.Database.SqlQuery<Subscribe>(
                $"""
                 SELECT * FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName}
                 """
            )
            .AsNoTracking()
            .SingleAsync();
    }

    public async Task<Result<List<Subscribe>?, Error>> GetSubscribes(int page, int pageSize, int userId)
    {
        page = page < 1 ? 1 : page;
        var data = await context.Database.SqlQuery<Subscribe>(
                $"""
                 SELECT
                   subscribe.*
                 FROM
                   subscribe
                 WHERE
                   "user_id" = { userId }
                 LIMIT { pageSize } OFFSET {( page - 1 ) * pageSize}
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        if (data.Count == 0)
        {
            return new Error(ErrorCode.NoData);
        }

        return data;
    }

    public async Task<Result<Unit, Error>> DeleteSubscribe(int userId, string journalName)
    {
        var collect = await context.Database.SqlQuery<Collect>(
                $"""
                 SELECT * FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName}
                 """
            )
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (collect is null)
        {
            return new Error(ErrorCode.InvalidCredentials);
        }
        
        await context.Database.ExecuteSqlAsync(
            $"""
             DELETE FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName};
             """
        );

        return new Unit();
    }
}