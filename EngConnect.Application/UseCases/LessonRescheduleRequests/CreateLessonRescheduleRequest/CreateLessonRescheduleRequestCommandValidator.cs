using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using FluentValidation;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest;

public class CreateLessonRescheduleRequestCommandValidator : AbstractValidator<CreateLessonRescheduleRequestCommand>
{
    public CreateLessonRescheduleRequestCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.LessonId)
                .NotEmpty()
                .WithMessage(ScheduleErrors.LessonNotFound().Message);

            RuleFor(x => x.Request.StudentId)
                .NotEmpty()
                .WithMessage(ScheduleErrors.StudentNotFound().Message);

            RuleFor(x => x.Request)
                .Must(x => x.ProposedStartTime < x.ProposedEndTime)
                .WithMessage(ScheduleErrors.InvalidTimeRange().Message);
        });
    }
}