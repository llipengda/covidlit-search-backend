using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class JournalService(DbprojectContext context) : IJournalService
{
    public async Task<List<Journal>> GetJournals(string search, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        return await context.Database.SqlQuery<Journal>(
            $"""
             SELECT * 
             FROM "journal" 
             WHERE "journal"."name" LIKE '%' || {search} || '%' 
             LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
             """
            )
            .AsNoTracking()
            .ToListAsync();

    }
}