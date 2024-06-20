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

public class ConnectionStrings
{
    public string DBProject { get; set; } = null!;
}

public static class AppSettings
{
    public static Jwt Jwt { get; set; } = new();

    public static Smtp Smtp { get; set; } = new();

    public static ConnectionStrings ConnectionStrings { get; set; } = new();

    private static ILogger _logger = LoggerFactory
        .Create(config => config.AddConsole())
        .CreateLogger(typeof(AppSettings));

    private static bool _hasError = false;

    public static void Init(IConfiguration configuration)
    {
        configuration.GetSection("Jwt").Bind(Jwt);
        configuration.GetSection("Smtp").Bind(Smtp);
        configuration.GetSection("ConnectionStrings").Bind(ConnectionStrings);

        Validate(Jwt.SecretKey, "Jwt:SecretKey");
        Validate(Smtp.Host, "Smtp:Host");
        Validate(Smtp.Port, "Smtp:Port");
        Validate(Smtp.Username, "Smtp:Username");
        Validate(Smtp.Password, "Smtp:Password");
        Validate(ConnectionStrings.DBProject, "ConnectionStrings:DBProject");

        if (!_hasError) return;

        _logger.LogError("AppSettings is not configured properly. If you are using Docker, use -e to set environment variables.");
        Environment.Exit(1);
    }

    private static void Validate(string value, string name)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        _hasError = true;
        _logger.LogError("{name} is not set properly", name);
    }

    private static void Validate(int value, string name)
    {
        if (value > 0)
        {
            return;
        }

        _hasError = true;
        _logger.LogError("{name} is not set properly", name);
    }
}