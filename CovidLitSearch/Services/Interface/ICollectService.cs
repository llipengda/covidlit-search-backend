using CovidLitSearch.Models;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Services.Interface;

public interface ICollectService
{
    Task<Result<Collect?, Error>> Collect(int userId, string articleId);
}