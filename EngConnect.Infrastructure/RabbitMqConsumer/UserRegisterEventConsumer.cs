using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Domain.Settings;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class UserRegisterEventConsumer : IConsumer<UserRegisterEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserRegisterEventConsumer> _logger;
    private readonly AppSettings _appSettings;

    public UserRegisterEventConsumer(IUnitOfWork unitOfWork, IEmailService emailService,
        ILogger<UserRegisterEventConsumer> logger, IOptions<AppSettings> appSettings)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    public async Task Consume(ConsumeContext<UserRegisterEvent> context)
    {
        _logger.LogInformation("Start CustomerRegisterEventConsumer {@EventData}", context.Message);
        try
        {
            var eventData = context.Message;

            var emailTemplate = await _unitOfWork.GetRepository<EmailTemplate, Guid>()
                .FindFirstAsync(x => x.EventType == eventData.EventType && x.Role == nameof(UserRoleEnum.User), false);

            if (emailTemplate == null)
            {
                _logger.LogWarning("No email template found for event type: {EventType} and role: {Role}",
                    eventData.EventType, nameof(UserRoleEnum.User));
                return;
            }

            // Implement sending email logic here using the emailTemplate and event data
            var emailContent = RenderTemplate(emailTemplate.Body, eventData);

            // Send verification email to the customer
            await _emailService.SendEmailAsync([eventData.Email], [], new EmailContent
            {
                Subject = emailTemplate.Subject,
                HtmlBody = emailContent
            });

            _logger.LogInformation("End CustomerRegisterEventConsumer");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error consuming CustomerRegisteredEvent: {Message}", e.Message);
        }
    }

    private string RenderTemplate(string template, UserRegisterEvent eventData)
    {
        return template
            .Replace("{{fullName}}", eventData.FullName)
            .Replace("{{feUrl}}", _appSettings.FrontendUrl)
            .Replace("{{verificationCode}}", eventData.VerificationToken);
    }
}