using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class SubscribeService(DbprojectContext context) : ISubscribeService
{
    public async Task<Result<Subscribe, Error>> Subscribe(int userId, string journalName)
    {
        var subscribe = await context
            .Database.SqlQuery<Subscribe>(
                $"""
                 SELECT * FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName}
                 """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (subscribe is not null)
        {
            return new Error(ErrorCode.AlreadySubscribed);
        }

        await context.Database.ExecuteSqlAsync(
            $"""
             INSERT INTO "subscribe" VALUES({userId}, {journalName});
             """
        );

        return await context
            .Database.SqlQuery<Subscribe>(
                $"""
                 SELECT * FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName}
                 """
            )
            .AsNoTracking()
            .SingleAsync();
    }

    public async Task<Result<List<Subscribe>, Error>> GetSubscribes(
        int page,
        int pageSize,
        int userId
    )
    {
        page = page < 1 ? 1 : page;
        var data = await context
            .Database.SqlQuery<Subscribe>(
                $"""
                 SELECT
                   subscribe.*
                 FROM
                   subscribe
                 WHERE
                   "user_id" = {userId}
                 ORDER BY "journal_name"
                 LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        if (data.Count == 0)
        {
            return new Error(ErrorCode.NotFound);
        }

        return data;
    }

    public async Task<Result<Error>> DeleteSubscribe(int userId, string journalName)
    {
        var subscribe = await context
            .Database.SqlQuery<Subscribe>(
                $"""
                 SELECT * FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName}
                 """
            )
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (subscribe is null)
        {
            return new Error(ErrorCode.InvalidCredentials);
        }

        await context.Database.ExecuteSqlAsync(
            $"""
             DELETE FROM "subscribe" WHERE user_id = {userId} AND journal_name = {journalName};
             """
        );

        return new();
    }
}