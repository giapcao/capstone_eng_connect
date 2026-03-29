using EngConnect.BuildingBlock.Application.Validation.Validation;
using FluentValidation;

namespace EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;

public class CreateSupportTicketMessageCommandValidator : AbstractValidator<CreateSupportTicketMessageCommand>
{
    public CreateSupportTicketMessageCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("TicketId is required.");

        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("SenderId is required.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(2000).WithMessage("Message must not exceed 2000 characters.");
    }
}