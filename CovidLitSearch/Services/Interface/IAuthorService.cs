using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Services.Interface;

public interface IAuthorService
{
    Task<Result<List<Author>, Error>> GetAuthors(string? search, int page, int pageSize, string? refine);

    Task<Result<Author?, Error>> GetAuthorById(string name);
    
    Task<Result<int, Error>> GetAuthorsCount(string? search, string? refine);
    
    Task<Result<List<ArticleDto>, Error>> GetArticlesByAuthor(string name, int page, int pageSize); 

}