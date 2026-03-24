using EngConnect.BuildingBlock.Application.Validation.Validation;
using FluentValidation;

namespace EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public class GetAiSummaryCommandValidator : AbstractValidator<GetAiSummaryCommand>
{
    public GetAiSummaryCommandValidator()
    {
        RuleFor(x => x.Transcript)
            .NotEmpty().WithMessage("Transcript không được để trống.");
    }
}
