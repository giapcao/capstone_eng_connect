using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.Domain.Persistence.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class LessonRescheduleRequestReviewedEventConsumer : IConsumer<LessonRescheduleRequestReviewedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<LessonRescheduleRequestReviewedEventConsumer> _logger;

    public LessonRescheduleRequestReviewedEventConsumer(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<LessonRescheduleRequestReviewedEventConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<LessonRescheduleRequestReviewedEvent> context)
    {
        _logger.LogInformation("Start LessonRescheduleRequestReviewedEventConsumer {@EventData}", context.Message);
        try
        {
            var eventData = context.Message;

            var emailTemplate = await _unitOfWork.GetRepository<EmailTemplate, Guid>()
                .FindFirstAsync(x => x.EventType == eventData.EventType && x.Role == nameof(UserRoleEnum.Student), false);

            if (emailTemplate is null)
            {
                _logger.LogWarning("No email template found for event type: {EventType} and role: {Role}",
                    eventData.EventType, nameof(UserRoleEnum.Student));
                return;
            }

            var emailBody = RenderTemplate(emailTemplate.Body, eventData);

            await _emailService.SendEmailAsync([eventData.StudentEmail], [], new EmailContent
            {
                Subject = emailTemplate.Subject,
                HtmlBody = emailBody
            });

            _logger.LogInformation("End LessonRescheduleRequestReviewedEventConsumer");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error consuming LessonRescheduleRequestReviewedEvent: {Message}", ex.Message);
        }
    }

    private static string RenderTemplate(string template, LessonRescheduleRequestReviewedEvent eventData)
    {
        return template
            .Replace("{{fullName}}", eventData.StudentFullName)
            .Replace("{{status}}", eventData.Status)
            .Replace("{{proposedStartTime}}", eventData.ProposedStartTime.ToString("yyyy-MM-dd HH:mm"))
            .Replace("{{proposedEndTime}}", eventData.ProposedEndTime.ToString("yyyy-MM-dd HH:mm"))
            .Replace("{{tutorNote}}", eventData.TutorNote ?? string.Empty);
    }
}
