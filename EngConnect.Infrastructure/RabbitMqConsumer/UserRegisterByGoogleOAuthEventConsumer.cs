using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Domain.Settings;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class UserRegisterByGoogleOAuthEventConsumer: IConsumer<UserRegisterByGoogleOAuthEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserRegisterByGoogleOAuthEventConsumer> _logger;
    private readonly AppSettings _appSettings;

    public UserRegisterByGoogleOAuthEventConsumer(
        IUnitOfWork unitOfWork, 
        IEmailService emailService, 
        ILogger<UserRegisterByGoogleOAuthEventConsumer> logger, 
        IOptions<AppSettings> appSettings)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    public async Task Consume(ConsumeContext<UserRegisterByGoogleOAuthEvent> context)
    {
        _logger.LogInformation("Start UserRegisterByGoogleOAuthEventConsumer {@Event}", context.Message);
        try
        {
            var eventData = context.Message;
            var emailTemplate = await _unitOfWork.GetRepository<EmailTemplate, Guid>()
                .FindFirstAsync(x => x.EventType == eventData.EventType && x.Role == nameof(UserRoleEnum.User),
                    tracking: false);
            
            //Check if template exists
            if (ValidationUtil.IsNullOrEmpty(emailTemplate))
            {
                _logger.LogWarning("No email template found for {@EventType}", eventData.EventType);
                return;
            }
            
            // Render email content
            var emailContent = RenderTemplate(emailTemplate.Body, eventData);
            
            //Send email
            await _emailService.SendEmailAsync([eventData.Email],[], new EmailContent
            {
                Subject = emailTemplate.Subject,
                HtmlBody = emailContent,
            });
            
            _logger.LogInformation("End UserRegisterByGoogleOAuthEventConsumer {@Event}", context.Message);
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error consuming UserRegisterByGoogleOAuthEventConsumer: {Message}", ex.Message);
        }
    }
    
    private string RenderTemplate(string template, UserRegisterByGoogleOAuthEvent eventData)
    {
        return template
            .Replace("{{fullName}}", eventData.FullName)
            .Replace("{{feUrl}}", _appSettings.FrontendUrl)
            .Replace("{{generatedPassword}}", eventData.GeneratedPassword);
    }
}