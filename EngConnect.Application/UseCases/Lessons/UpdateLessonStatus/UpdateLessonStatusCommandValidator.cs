using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;

public class UpdateLessonStatusCommandValidator : AbstractValidator<UpdateLessonStatusCommand>
{
    public UpdateLessonStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || Enum.TryParse<LessonStatus>(status, true, out _))
            .WithMessage("Trạng thái không hợp lệ");
    }
}
