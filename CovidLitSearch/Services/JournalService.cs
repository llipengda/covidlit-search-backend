using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
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
}