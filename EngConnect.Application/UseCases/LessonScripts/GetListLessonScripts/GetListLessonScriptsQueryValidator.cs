using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;

public class GetListLessonScriptsQueryValidator : AbstractValidator<GetListLessonScriptsQuery>
{
    public GetListLessonScriptsQueryValidator()
    {
        RuleFor(x => x.Language)
            .Must(language => string.IsNullOrEmpty(language) || Enum.TryParse<Language>(language, true, out _))
            .WithMessage("Trạng thái không hợp lệ");
    }
}
