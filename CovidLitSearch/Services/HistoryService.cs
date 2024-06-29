using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class HistoryService(DbprojectContext context) : IHistoryService
{
    public async Task<Result<List<HistoryDto>, Error>> GetHistory(
        int userId,
        int page,
        int pageSize
    )
    {
        page = page < 1 ? 1 : page;
        var data = await context
            .Database.SqlQuery<HistoryDto>(
                $"""
                 SELECT
                   history.*,
                   title,
                   authors,
                   abstract,
                   journal_name 
                 FROM
                   "history"
                   JOIN article ON history.article_id = article."id"
                   JOIN publish ON publish.article_id = history.article_id 
                 WHERE
                   "user_id" = {userId}
                 ORDER BY
                   "time" DESC
                 LIMIT {pageSize} OFFSET {(page - 1) * pageSize} 
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data;
    }
    
    public async Task<Result<int, Error>> GetHistoryCount(
        int userId
    )
    {
        var data = await context
            .Database.SqlQuery<CountType>(
                $"""
                 SELECT
                   COUNT(*) AS "count"
                 FROM
                   "history"
                 WHERE
                   "user_id" = {userId}
                 ORDER BY
                   "time" DESC
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data.Count;
    }
}