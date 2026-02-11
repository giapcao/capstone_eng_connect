using EngConnect.Domain.DomainErrors;
using FluentValidation;

namespace EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule;

public class DeleteTutorScheduleCommandValidator : AbstractValidator<DeleteTutorScheduleCommand>
{
    public DeleteTutorScheduleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ScheduleErrors.TutorScheduleNotFound().Message);
    }
}