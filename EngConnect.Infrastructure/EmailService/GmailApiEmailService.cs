using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Contracts.Settings;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EngConnect.Infrastructure.EmailService;

public class GmailApiEmailService : IEmailService
{
    private readonly GmailService _gmailService;
    private readonly GmailApiSettings _gmailApiSettings;
    private readonly ILogger<GmailApiEmailService> _logger;

    public GmailApiEmailService(
        GmailService gmailService,
        IOptions<GmailApiSettings> gmailApiSettings,
        ILogger<GmailApiEmailService> logger)
    {
        _gmailService = gmailService;
        _gmailApiSettings = gmailApiSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(List<string> to, List<string> cc, EmailContent content)
    {
        try
        {
            var email = new MimeMessage();
            var senderName = string.IsNullOrWhiteSpace(_gmailApiSettings.SenderName)
                ? _gmailApiSettings.SenderEmail
                : _gmailApiSettings.SenderName;

            email.From.Add(new MailboxAddress(senderName, _gmailApiSettings.SenderEmail));
            email.To.AddRange(to.Select(MailboxAddress.Parse));

            if (cc.Count != 0)
            {
                email.Cc.AddRange(cc.Select(MailboxAddress.Parse));
            }

            if (!string.IsNullOrWhiteSpace(content.ReplyTo))
            {
                email.ReplyTo.Add(MailboxAddress.Parse(content.ReplyTo));
            }

            email.Subject = content.Subject;
            var bodyBuilder = new BodyBuilder { HtmlBody = content.HtmlBody };

            if (!string.IsNullOrEmpty(content.PlainTextBody))
            {
                bodyBuilder.TextBody = content.PlainTextBody;
            }

            email.Body = bodyBuilder.ToMessageBody();

            using var stream = new MemoryStream();
            await email.WriteToAsync(stream);

            var rawMessage = Convert.ToBase64String(stream.ToArray())
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            var message = new Message { Raw = rawMessage };
            await _gmailService.Users.Messages.Send(message, "me").ExecuteAsync();

            _logger.LogInformation("Email sent successfully to: {Recipients}", string.Join(", ", to));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Gmail API email: {Message}", ex.Message);
        }
    }
}
