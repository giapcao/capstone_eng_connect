// using EngConnect.BuildingBlock.Contracts.Abstraction;
// using EngConnect.BuildingBlock.Contracts.Models.Email;
// using EngConnect.BuildingBlock.Domain.Constants;
// using EngConnect.BuildingBlock.EventBus.Events;
// using EngConnect.Domain.Settings;
// using MassTransit;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
//
// namespace EngConnect.Infrastructure.RabbitMqConsumer;
//
// public class ResetPasswordEventConsumer : IConsumer<ResetPasswordEvent>
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IEmailService _emailService;
//     private readonly ILogger<CustomerRegisterEventConsumer> _logger;
//     private readonly AppSettings _appSettings;
//
//     public ResetPasswordEventConsumer(IUnitOfWork unitOfWork, IEmailService emailService,
//         ILogger<CustomerRegisterEventConsumer> logger, IOptions<AppSettings> appSettings)
//     {
//         _unitOfWork = unitOfWork;
//         _emailService = emailService;
//         _logger = logger;
//         _appSettings = appSettings.Value;
//     }
//
//     public async Task Consume(ConsumeContext<ResetPasswordEvent> context)
//     {
//         _logger.LogInformation("Start CustomerRegisterEventConsumer {@EventData}", context.Message);
//         try
//         {
//             var eventData = context.Message;
//
//             var emailTemplate = await _unitOfWork.GetRepository<EmailTemplate, Guid>()
//                 .FindFirstAsync(x => x.EventType == eventData.EventType && x.Role == nameof(UserRole.Customer), false);
//
//             if (emailTemplate == null)
//             {
//                 _logger.LogWarning("No email template found for event type: {EventType} and role: {Role}",
//                     eventData.EventType, nameof(UserRole.Customer));
//                 return;
//             }
//
//             // Implement sending email logic here using the emailTemplate and event data
//             var emailContent = RenderTemplate(emailTemplate.Body, eventData);
//
//             // Send reset password email to the customer
//             await _emailService.SendEmailAsync([eventData.Email], [], new EmailContent
//             {
//                 Subject = emailTemplate.Subject,
//                 HtmlBody = emailContent
//             });
//
//             _logger.LogInformation("End CustomerRegisterEventConsumer");
//         }
//         catch (Exception e)
//         {
//             _logger.LogError(e, "Error consuming ResetPasswordEvent: {Message}", e.Message);
//         }
//     }
//
//     private string RenderTemplate(string template, ResetPasswordEvent eventData)
//     {
//         return template
//             .Replace("{{fullName}}", eventData.FullName)
//             .Replace("{{feUrl}}", _appSettings.FrontendUrl)
//             .Replace("{{resetPasswordToken}}", eventData.ResetPasswordToken);
//     }
// }