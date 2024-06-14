using CovidLitSearch.Models;
using CovidLitSearch.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Services;

public class AuthorService(DbprojectContext context) : IAuthorService
{
    public async Task<List<Author>> GetAuthors(string? search, int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        return await context.Database.SqlQuery<Author>(
            $"""
             SELECT * FROM "author" WHERE "name" LIKE '%'|| {search} || '%' LIMIT {pageSize} OFFSET {(page - 1) * pageSize}
             """
            )
            .AsNoTracking()
            .ToListAsync();
    }
}