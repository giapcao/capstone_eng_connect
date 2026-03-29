using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.Domain.Persistence.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class CourseVerificationReviewedEventConsumer : IConsumer<CourseVerificationReviewedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<CourseVerificationReviewedEventConsumer> _logger;

    public CourseVerificationReviewedEventConsumer(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<CourseVerificationReviewedEventConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CourseVerificationReviewedEvent> context)
    {
        _logger.LogInformation("Start CourseVerificationReviewedEventConsumer {@EventData}", context.Message);
        try
        {
            var eventData = context.Message;

            var emailTemplate = await _unitOfWork.GetRepository<EmailTemplate, Guid>()
                .FindFirstAsync(x => x.EventType == eventData.EventType && x.Role == nameof(UserRoleEnum.Tutor), false);

            if (emailTemplate is null)
            {
                _logger.LogWarning("No email template found for event type: {EventType} and role: {Role}",
                    eventData.EventType, nameof(UserRoleEnum.Tutor));
                return;
            }

            var emailBody = RenderTemplate(emailTemplate.Body, eventData);

            await _emailService.SendEmailAsync([eventData.TutorEmail], [], new EmailContent
            {
                Subject = emailTemplate.Subject,
                HtmlBody = emailBody
            });

            _logger.LogInformation("End CourseVerificationReviewedEventConsumer");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error consuming CourseVerificationReviewedEvent: {Message}", ex.Message);
        }
    }

    private static string RenderTemplate(string template, CourseVerificationReviewedEvent eventData)
    {
        return template
            .Replace("{{fullName}}", eventData.TutorFullName)
            .Replace("{{courseTitle}}", eventData.CourseTitle)
            .Replace("{{status}}", eventData.Status)
            .Replace("{{rejectionReason}}", eventData.RejectionReason ?? string.Empty);
    }
}
