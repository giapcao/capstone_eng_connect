using EngConnect.Application.UseCases.TutorSchedules.Extensions;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using FluentValidation;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest;

public class UpdateLessonRescheduleRequestCommandValidator : AbstractValidator<UpdateLessonRescheduleRequestCommand>
{
    public UpdateLessonRescheduleRequestCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage(CommonErrors.ValidationFailed("Dữ liệu không thể null.").Message);

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty()
                .WithMessage(ScheduleErrors.RescheduleRequestNotFound().Message);

            RuleFor(x => x.Request.Status)
                .NotEmpty()
                .WithMessage(CommonErrors.ValidationFailed("Status không được để trống.").Message)
                .Must(ScheduleStatusExtensions.IsValidLessonRescheduleRequestStatus)
                .WithMessage(CommonErrors.ValidationFailed("Status không hợp lệ.").Message);
        });
    }
}