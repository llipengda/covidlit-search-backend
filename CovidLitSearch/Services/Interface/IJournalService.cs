using CovidLitSearch.Models;

namespace CovidLitSearch.Services.Interface;

public interface IJournalService
{
    Task<List<Journal>> GetJournals(string search, int page, int pageSize);
}