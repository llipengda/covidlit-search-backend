using CovidLitSearch.Models.Enums;

namespace CovidLitSearch.Models.Common;

public class Error
{
    public ErrorCode Code { get; set; }

    public string? Message { get; set; }

    public Error(ErrorCode code)
    {
        Code = code;
        Message = Enum.GetName(code);
    }

    public Error(ErrorCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public override string ToString()
    {
        return $"Error {Code}: {Message}";
    }
}
