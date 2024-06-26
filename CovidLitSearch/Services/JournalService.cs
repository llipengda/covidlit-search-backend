using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class JournalService(DbprojectContext context) : IJournalService
{
    public async Task<Result<List<Journal>, Error>> GetJournals(
        string search,
        int page,
        int pageSize
    )
    {
        page = page <= 0 ? 1 : page;
        var data = await context
            .Database.SqlQuery<Journal>(
                $"""
                 SELECT * 
                 FROM "journal" 
                 WHERE "journal"."name" LIKE '%' || {search} || '%' 
                 ORDER BY "journal"."name"
                 LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
                 """
            )
            .AsNoTracking()
            .ToListAsync();

        return data;
    }

    public async Task<Result<Journal?, Error>> GetJournalById(string name)
    {
        var data = await context.Database.SqlQuery<Journal>(
            $"""
             SELECT * 
             FROM "journal" 
             WHERE "journal"."name" = {name}
             """
        ).AsNoTracking().SingleOrDefaultAsync();
        
        if (data is null)
        {
            return new Error(ErrorCode.NotFound);
        }

        return data;
    }

    public async Task<Result<int, Error>> GetJournalsCount(string? search)
    {
        var count = await context.Database.SqlQuery<CountType>(
            $"""
             SELECT COUNT(*) 
             FROM "journal" 
             WHERE "journal"."name" LIKE '%' || {search} || '%'
             """
        ).AsNoTracking().SingleOrDefaultAsync();

        return new Result<int, Error>(count!.Count);
    }

    public async Task<Result<List<ArticleDto>, Error>> GetArticlesByJournal(string name, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        var data = await context.Database.SqlQuery<Journal>(
            $"""
             SELECT * 
             FROM "journal" 
             WHERE "journal"."name" = {name}
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
                publish 
                JOIN article ON publish.article_id = article.id
             WHERE 
               publish.journal_name = {name}
             ORDER BY 
               article.publish_time DESC
             LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
             """
        ).AsNoTracking().ToListAsync();

        return articles;
    }
}