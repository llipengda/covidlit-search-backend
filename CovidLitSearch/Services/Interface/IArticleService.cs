using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;

namespace CovidLitSearch.Services.Interface;

public interface IArticleService
{
    Task<List<ArticleDto>> GetArticles(
        int page,
        int pageSize,
        bool allowNoUrl,
        string? search,
        ArticleSearchBy? searchBy
    );

    Task<ArticleDto?> GetArticleById(string id);

    Task<List<ArticleDto>> GetArticlesByResearch(
        int page,
        int pageSize,
        string? studyType, 
        string? addressedPopulation,
        string? challenge,
        string? focus
    );
    
    Task<List<CiteDto>> GetCites(int page, int pageSize, string id);
    
}
