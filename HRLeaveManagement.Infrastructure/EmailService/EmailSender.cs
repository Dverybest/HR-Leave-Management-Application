using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HRLeaveManagement.Infrastructure.EmailService;

public class EmailSender : IEmailSender
{
    public EmailSettings EmailSettings { get; }
    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        EmailSettings = emailSettings.Value;
    }

    public async Task<bool> SendEmail(EmailMessage email)
    {

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(EmailSettings.FromName, EmailSettings.FormAddress));
        message.To.Add(new MailboxAddress("", email.To));
        message.Subject = email.Subject;
        message.Body = new TextPart("plain") { Text = email.Body };

        using var client = new SmtpClient();
        client.Connect(EmailSettings.Host, EmailSettings.Port, SecureSocketOptions.StartTls);
        client.Authenticate(EmailSettings.FormAddress, EmailSettings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        return true;
    }
}
