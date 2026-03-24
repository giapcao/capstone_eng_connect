using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.Domain.Persistence.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class WarningInvalidLessonEventConsumer : IConsumer<WarningInvalidLessonEvent>
{
    private readonly ILogger<WarningInvalidLessonEventConsumer> _logger;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public WarningInvalidLessonEventConsumer(IUnitOfWork unitOfWork, IEmailService emailService, ILogger<WarningInvalidLessonEventConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<WarningInvalidLessonEvent> context)
    {
        _logger.LogInformation("Processing WarningInvalidLessonEvent: {EventId}", context.MessageId);
        
        try
        {
            var eventData = context.Message;

            var emailTemplate = await _unitOfWork.GetRepository<EmailTemplate, Guid>()
                .FindFirstAsync(x => x.EventType == eventData.EventType && x.Role == nameof(UserRoleEnum.User), false);

            if (emailTemplate == null)
            {
                _logger.LogWarning("Email Template not found for EventType: {EventType}", eventData.EventType);
                return;
            }
            
            var emailContent = RenderTemplate(emailTemplate.Body, eventData);
            var emailSubject = emailTemplate.Subject;

            var recipients = new List<string?> { eventData.StudentEmail, eventData.TutorEmail }
                .Where(email => !string.IsNullOrWhiteSpace(email))
                .Distinct()
                .ToList();

            if (!recipients.Any())
            {
                _logger.LogWarning("No valid recipient emails found for Event: {EventId}", context.MessageId);
                return;
            }
            
            var emailTasks = recipients.Select(email => 
                _emailService.SendEmailAsync([email!], [], new EmailContent
                {
                    Subject = emailSubject,
                    HtmlBody = emailContent
                }));

            await Task.WhenAll(emailTasks);
            
            _logger.LogInformation("Successfully processed WarningInvalidLessonEvent: {EventId}", context.MessageId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process WarningInvalidLessonEvent {EventId}: {Message}", context.MessageId, e.Message);
            throw; 
        }
    }
    
    private string RenderTemplate(string template, WarningInvalidLessonEvent eventData)
    {
        return template
            .Replace("{{studentName}}", eventData.StudentName)
            .Replace("{{tutorName}}", eventData.TutorName)
            .Replace("{{quality}}", $"{eventData.QualityPercentage:F1}%") 
            .Replace("{{startTime}}", eventData.StartTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A")
            .Replace("{{endTime}}", eventData.EndTime?.ToString("dd/MM/yyyy HH:mm") ?? "N/A")
            .Replace("{{passContent}}", GetContentOrEmpty(eventData.Reason.Pass))
            .Replace("{{failContent}}", GetContentOrEmpty(eventData.Reason.Fail));
    }

    private string GetContentOrEmpty(List<string>? items)
    {
        if (items == null || !items.Any())
        {
            return "<i style='color: #94a3b8;'>(Không có thông tin ghi nhận)</i>";
        }

        var listItems = items.Select(item => $"<li style='margin-bottom: 6px;'>{item}</li>");
        
        return $"<ul style='margin: 0; padding-left: 20px; list-style-type: disc;'>{string.Join("", listItems)}</ul>";
    }
}