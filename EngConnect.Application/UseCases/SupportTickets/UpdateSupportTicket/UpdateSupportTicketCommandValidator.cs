using EngConnect.BuildingBlock.Application.Validation.Validation;
using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;

public class UpdateSupportTicketCommandValidator : AbstractValidator<UpdateSupportTicketCommand>
{
    public UpdateSupportTicketCommandValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject không được để trống.")
            .MaximumLength(200).WithMessage("Subject không được quá 200 kí tự.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description không được quá 1000 kí tự.");

        RuleFor(x => x.Type)
            .Must(type => string.IsNullOrEmpty(type) || Enum.TryParse<TypeSupportTicket>(type, true, out _))
            .WithMessage("Loại ticket không hợp lệ");
    }
}