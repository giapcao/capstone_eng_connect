using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;

public class UpdateSupportTicketStatusCommandValidator : AbstractValidator<UpdateSupportTicketStatusCommand>
{

    public UpdateSupportTicketStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống.");
        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || Enum.TryParse<SupportTicketStatus>(status, true, out _))
            .WithMessage("Trạng thái không hợp lệ");
    }
}