using EngConnect.BuildingBlock.Application.Validation.Validation;
using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.Lessons.GetListLessons;

public class GetListLessonValidator : AbstractValidator<GetListLessonQuery>
{
    public GetListLessonValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || Enum.IsDefined(typeof(LessonStatus), status))
            .WithMessage("Trạng thái không hợp lệ");
    }
}
