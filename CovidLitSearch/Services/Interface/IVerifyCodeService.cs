using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Services.Interface;

public interface IVerifyCodeService
{
    Result<Error> Send(string email);

    bool Verify(string email, int code);
}
