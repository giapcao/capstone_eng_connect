using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using FluentValidation;

namespace EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

public class UpdateTutorScheduleCommandValidator : AbstractValidator<UpdateTutorScheduleCommand>
{
    public UpdateTutorScheduleCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty()
                .WithMessage(ScheduleErrors.TutorScheduleNotFound().Message);

            RuleFor(x => x.Request.Weekday)
                .NotEmpty()
                .WithMessage(CommonErrors.ValidationFailed("Weekday không được để trống.").Message);

            RuleFor(x => x.Request)
                .Must(x => x.StartTime < x.EndTime)
                .WithMessage(ScheduleErrors.InvalidTimeRange().Message);
        });
    }
}