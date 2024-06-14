using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.Enums;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace CovidLitSearch.Services;

public class VerifyCodeService(IMemoryCache cache) : IVerifyCodeService
{
    public Result<Error> Send(string email)
    {
        if (cache.TryGetValue(email, out _))
        {
            return new Error(ErrorCode.CodeAlreadySent);
        }
        var code = GenerateCode();
        var body = $"""
            <!DOCTYPE html>
            <html>
              <head>
                <title>Verification Code</title>
              </head>
              <body style="font-family: Arial; color: #333333">
                <div
                  style="
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                  "
                >
                  <h2 style="color: #b70031">Hello，<span style="font-size: 16px;">{email}</span></h2>
                  <p>
                    Your verification code is：
                    <span style="font-size: 24px; color: #b70031; font-weight: 700">{code}</span>
                  </p>
                  <p>It will exprie in 5 minutes.</p>
                  <p style="font-size: 14px; color: #888888; margin-top: 20px">
                    CovidLitSearch
                  </p>
                </div>
              </body>
            </html>
            """;
        EmailUtil.SendEmail(email, "Verification Code", body);
        cache.Set(
            email,
            code,
            new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(5) }
        );
        return new();
    }

    public bool Verify(string email, int code)
    {
        cache.TryGetValue(email, out int? storedCode);
        if (code == storedCode)
        {
            cache.Remove(email);
        }
        return code == storedCode;
    }

    private static int GenerateCode()
    {
        return new Random().Next(0, 999999);
    }
}
