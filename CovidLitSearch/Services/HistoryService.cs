using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class HistoryService(DbprojectContext context) : IHistoryService
{
    public async Task<List<HistoryDto>> GetHistory(int userId, int page, int pageSize)
    {
        return await context.Database
            .SqlQuery<HistoryDto>(
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
                 LIMIT {pageSize} OFFSET {( page - 1 ) * pageSize} 
                   
                 """
            )
            .AsNoTracking()
            .ToListAsync();
    }
}