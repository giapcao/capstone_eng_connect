using EngConnect.BuildingBlock.Application.Validation.Validation;
using FluentValidation;

namespace EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public class GetAiSummaryCommandValidator : AbstractValidator<GetAiSummaryCommand>
{
    public GetAiSummaryCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty().WithMessage("LessonId không được để trống.");
    }
}
