using System.Net;
using System.Net.Mail;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Utilities;

public static class EmailUtil
{
    private static readonly ILogger _logger;

    static EmailUtil()
    {
        _logger = LoggerFactory
            .Create(options => options.AddConsole())
            .CreateLogger(typeof(EmailUtil));
    }
    
    public static void SendEmail(string email, string subject, string body)
    {
        var client = new SmtpClient
        {
            Host = AppSettings.Smtp.Host,
            Port = AppSettings.Smtp.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                AppSettings.Smtp.Username,
                AppSettings.Smtp.Password
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress(AppSettings.Smtp.Username, "CovidLit Search"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        message.To.Add(email);

        client.SendCompleted += (_, e) =>
        {
            if (e.Error is null)
            {
                _logger.LogInformation("Successfully sent email to [{email}]", email);
            }
            else
            {
                _logger.LogError(e.Error, "Failed to send email to [{email}]", email);
            }
        };

        client.SendAsync(message, email);
    }
}