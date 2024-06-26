using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;

namespace CovidLitSearch.Services.Interface;

public interface IArticleService
{
    Task<Result<List<ArticleDto>, Error>> GetArticles(
        int page,
        int pageSize,
        bool allowNoUrl,
        string? search,
        ArticleSearchBy? searchBy,
        string? orderBy,
        bool? desc
    );
    
    Task<Result<int, Error>> GetArticlesCount(bool allowNoUrl, string? search, ArticleSearchBy? searchBy);

    Task<Result<ArticleDto, Error>> GetArticleById(string id, int? userId);

    Task<Result<List<ArticleDto>, Error>> GetArticlesByResearch(
        int page,
        int pageSize,
        string? studyType,
        string? addressedPopulation,
        string? challenge,
        string? focus
    );

    Task<Result<List<CiteDto>, Error>> GetCites(int page, int pageSize, string id);
}
