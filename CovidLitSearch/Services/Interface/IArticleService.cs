using CovidLitSearch.Models.DTO;
using CovidLitSearch.Models.Enums;

namespace CovidLitSearch.Services.Interface;

public interface IArticleService
{
    Task<List<ArticleDTO>> GetArticles(
        int page,
        int pageSize,
        bool allowNoUrl,
        string? search,
        ArticleSearchBy? searchBy
    );

    Task<ArticleDTO?> GetArticleById(string id);
}
