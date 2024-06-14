using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
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

    public async Task<Result<List<CollectDto>?, Error>> GetCollects(int page, int pageSize, int userId)
    {
        page = page < 1 ? 1 : page;
        var data = await context.Database.SqlQuery<CollectDto>(
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

    public Task<Result<Collect?, Error>> DeleteCollect(int userId, string articleId)
    {
        throw new NotImplementedException();
    }
}