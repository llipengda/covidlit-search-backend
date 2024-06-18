using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Services.Interface;

public interface ICodeService
{
    Result<Error> Send(string email);

    Result<bool, Error> Verify(string email, int code);
}
