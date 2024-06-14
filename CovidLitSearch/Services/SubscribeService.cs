using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Services.Interface;

namespace CovidLitSearch.Services;

public class SubscribeService(DbprojectContext context) : ISubscribeService
{
    public Task<Result<Subscribe?, Error>> Subscribe(int userId, string journalId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<Subscribe>?, Error>> GetSubscribes(int page, int pageSize, int userId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit, Error>> DeleteSubscribe(int userId, string journalId)
    {
        throw new NotImplementedException();
    }
}