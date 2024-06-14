using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Services.Interface;

public interface IHistoryService
{
    
    Task<List<HistoryDto>> GetHistory(int userId, int page, int pageSize);
    
}