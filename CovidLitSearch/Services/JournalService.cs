using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class JournalService(DbprojectContext context) : IJournalService
{
    public async Task<Result<List<Journal>, Error>> GetJournals(string search, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        var data =  await context.Database.SqlQuery<Journal>(
            $"""
             SELECT * 
             FROM "journal" 
             WHERE "journal"."name" LIKE '%' || {search} || '%' 
             LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
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
}