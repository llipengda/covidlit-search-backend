using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.DTO;

namespace CovidLitSearch.Services.Interface;

public interface ICollectService
{
    Task<Result<Collect?, Error>> Collect(int userId, string articleId);
    
    Task<Result<List<CollectDto>?, Error> > GetCollects(int page, int pageSize, int userId);
    
    Task<Result<Unit, Error>> DeleteCollect(int userId, string articleId);
}