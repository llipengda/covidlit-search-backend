using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Services.Interface;

public interface ISubscribeService
{
    Task<Result<Subscribe, Error>> Subscribe(int userId, string journalName);
    
    Task<Result<List<Subscribe>, Error>> GetSubscribes(int page, int pageSize, int userId);
    
    Task<Result<Error>> DeleteSubscribe(int userId, string journalName);
    
    Task<Result<bool, Error>> IsSubscribed(int userId, string journalName);
}