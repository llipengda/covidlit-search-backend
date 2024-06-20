namespace CovidLitSearch.Models.Common;

public class Jwt
{
    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public string SecretKey { get; set; } = null!;
}

public class Smtp
{
    public string Host { get; set; } = null!;

    public int Port { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public static class AppSettings
{
    public static Jwt Jwt { get; set; } = new();

    public static Smtp Smtp { get; set; } = new();

    public static void Init(IConfiguration configuration)
    {
        configuration.GetSection("Jwt").Bind(Jwt);
        configuration.GetSection("Smtp").Bind(Smtp);
    }
}
