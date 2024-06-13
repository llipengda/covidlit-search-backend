using CovidLitSearch.Models;
using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Services.Interface;

public interface IArticleService
{
    Task<List<ArticleDTO>> GetArticles(int page, int pageSize);

    Task<Article?> GetArticleById(string id);
}
