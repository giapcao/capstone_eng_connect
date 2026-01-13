using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Contracts.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EngConnect.Infrastructure.EmailService;

public class MailKitEmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<MailKitEmailService> _logger;

    public MailKitEmailService(IOptions<EmailSettings> emailSettings, ILogger<MailKitEmailService> logger)
    {
        _logger = logger;
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(List<string> to, List<string> cc, EmailContent content)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.AddRange(to.Select(x => new MailboxAddress("", x)));

            if (cc.Count != 0)
            {
                email.Cc.AddRange(cc.Select(x => new MailboxAddress("", x)));
            }

            if (!string.IsNullOrEmpty(content.ReplyTo))
            {
                email.ReplyTo.Add(new MailboxAddress("", content.ReplyTo));
            }

            email.Subject = content.Subject;
            var builder = new BodyBuilder { HtmlBody = content.HtmlBody };

            if (!string.IsNullOrEmpty(content.PlainTextBody))
            {
                builder.TextBody = content.PlainTextBody;
            }

            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort,
                _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            _logger.LogInformation("Email sent successfully to: {Recipients}", string.Join(", ", to));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending email: {Message}", e.Message);
        }
    }
}