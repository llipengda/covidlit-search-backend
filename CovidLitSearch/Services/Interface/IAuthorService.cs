using CovidLitSearch.Models;

namespace CovidLitSearch.Services.Interface;

public interface IAuthorService
{
    Task<List<Author>> GetAuthors(string? search, int page, int pageSize);
    
}