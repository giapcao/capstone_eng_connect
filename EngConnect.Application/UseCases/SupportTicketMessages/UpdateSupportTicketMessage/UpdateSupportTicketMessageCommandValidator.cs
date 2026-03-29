using EngConnect.BuildingBlock.Application.Validation.Validation;
using FluentValidation;

namespace EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;

public class UpdateSupportTicketMessageCommandValidator : AbstractValidator<UpdateSupportTicketMessageCommand>
{
    public UpdateSupportTicketMessageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(2000).WithMessage("Message must not exceed 2000 characters.");
    }
}