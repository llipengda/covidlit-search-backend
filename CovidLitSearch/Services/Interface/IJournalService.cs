using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Services.Interface;

public interface IJournalService
{
    Task<Result<List<Journal>, Error>> GetJournals(string search, int page, int pageSize);
    
    Task<Result<Journal?, Error>> GetJournalById(string name);
    
}