using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Services.Interface;

public interface IHistoryService
{
    
    Task<Result<List<HistoryDto>, Error>> GetHistory(int userId, int page, int pageSize);
    
}