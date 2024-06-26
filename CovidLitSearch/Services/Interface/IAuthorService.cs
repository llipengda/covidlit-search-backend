using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Services.Interface;

public interface IAuthorService
{
    Task<Result<List<Author>, Error>> GetAuthors(string? search, int page, int pageSize);

    Task<Result<Author?, Error>> GetAuthorById(string name);
    
    Task<Result<int, Error>> GetAuthorsCount(string? search);

}