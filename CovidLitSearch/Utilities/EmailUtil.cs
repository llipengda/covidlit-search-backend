using System.Net;
using System.Net.Mail;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Utilities;

public static class EmailUtil
{
    public static void SendEmail(string email, string subject, string body, ILogger logger)
    {
        var client = new SmtpClient
        {
            Host = AppSettings.Email.Host,
            Port = AppSettings.Email.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                AppSettings.Email.Username,
                AppSettings.Email.Password
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress(AppSettings.Email.Username, "CovidLit Search"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        message.To.Add(email);

        client.SendCompleted += (_, e) =>
        {
            if (e.Error is null)
            {
                logger.LogInformation("Successfully sent email to [{email}]", email);
            }
            else
            {
                logger.LogError(e.Error, "Failed to send email to [{email}]", email);
            }
        };

        client.SendAsync(message, email);
    }
}
