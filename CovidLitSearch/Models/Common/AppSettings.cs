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

    public static string ConnectionString { get; set; } = null!;


    public static void Init(IConfiguration configuration)
    {
        configuration.GetSection("Jwt").Bind(Jwt);
        configuration.GetSection("Smtp").Bind(Smtp);
        ConnectionString = configuration.GetConnectionString("DBProject")!;

        var logger = LoggerFactory
            .Create(config => config.AddConsole())
            .CreateLogger(typeof(AppSettings));
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Jwt.SecretKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(Smtp.Host);
            ArgumentException.ThrowIfNullOrWhiteSpace(Smtp.Username);
            ArgumentException.ThrowIfNullOrWhiteSpace(Smtp.Password);
            ArgumentException.ThrowIfNullOrWhiteSpace(ConnectionString);
            if (Smtp.Port < 0)
            {
                throw new ArgumentNullException(nameof(Smtp.Port));
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "AppSettings is not configured properly. If you are using Docker, use -e to set environment variables.");
        }
    }
}