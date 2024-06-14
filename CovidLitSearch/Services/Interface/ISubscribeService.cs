using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Services.Interface;

public interface ISubscribeService
{
    Task<Result<Subscribe?, Error>> Subscribe(int userId, string journalId);
    
    Task<Result<List<Subscribe>?, Error>> GetSubscribes(int page, int pageSize, int userId);
    
    Task<Result<Unit, Error>> DeleteSubscribe(int userId, string journalId);
}