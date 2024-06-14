using System.Net;
using System.Net.Mail;
using CovidLitSearch.Models.Common;

namespace CovidLitSearch.Utilities;

public class EmailUtil
{
    public static void SendEmail(string email, string subject, string body)
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
            From = new MailAddress(AppSettings.Email.Username),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        message.To.Add(email);

        client.SendAsync(message, Guid.NewGuid().ToString());
    }
}
