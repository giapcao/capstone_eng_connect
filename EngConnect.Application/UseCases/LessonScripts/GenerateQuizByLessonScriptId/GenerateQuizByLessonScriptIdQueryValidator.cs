using FluentValidation;

namespace EngConnect.Application.UseCases.LessonScripts.GenerateQuizByLessonScriptId;

public class GenerateQuizByLessonScriptIdQueryValidator : AbstractValidator<GenerateQuizByLessonScriptIdQuery>
{
    public GenerateQuizByLessonScriptIdQueryValidator()
    {
        RuleFor(x => x.LessonScriptId)
            .NotEmpty().WithMessage("LessonScriptId không được để trống");
    }
}