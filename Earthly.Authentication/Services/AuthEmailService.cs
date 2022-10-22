using System.Net;
using Earthly.Authentication.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Earthly.Authentication.Services;

public class AuthEmailService : IAuthEmailService
{
    private readonly EmailSettings _emailSettings;

    public AuthEmailService(IConfiguration configuration, EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task SendAsync(string mailTo, string subject, string message, bool isHtml)
    {
         //create message
         var email = new MimeMessage();
         email.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
         email.To.Add(MailboxAddress.Parse(mailTo));
         email.Subject = subject;
         email.Body = new TextPart(TextFormat.Text) { Text = message };

         using var smtp = new SmtpClient();
         await smtp.ConnectAsync(_emailSettings.Server, 587, SecureSocketOptions.StartTls);
         await smtp.AuthenticateAsync(_emailSettings.Account, _emailSettings.Password);
         await smtp.SendAsync(email);
         await smtp.DisconnectAsync(true);

    }
}