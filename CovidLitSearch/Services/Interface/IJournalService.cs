using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Services.Interface;

public interface IJournalService
{
    Task<Result<List<Journal>, Error>> GetJournals(string search, int page, int pageSize, string? refine);
    
    Task<Result<Journal?, Error>> GetJournalById(string name);
    
    Task<Result<int, Error>> GetJournalsCount(string? search, string? refine);
    
    Task<Result<List<ArticleDto>, Error> > GetArticlesByJournal(string name, int page, int pageSize); 
    
}