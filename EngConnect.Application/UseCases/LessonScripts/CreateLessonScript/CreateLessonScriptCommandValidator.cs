using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;

public class CreateLessonScriptCommandValidator : AbstractValidator<CreateLessonScriptCommand>
{
    public CreateLessonScriptCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty()
            .WithMessage("LessonId không được để trống");

        RuleFor(x => x.RecordId)
            .NotEmpty()
            .WithMessage("RecordId không được để trống");

        RuleFor(x => x.Language)
            .Must(status => string.IsNullOrEmpty(status) || Enum.TryParse<Language>(status, true, out _))
            .WithMessage("Trạng thái không hợp lệ");

        RuleFor(x => x.FullText)
            .MaximumLength(5000)
            .WithMessage("FullText không được nhập quá 5000 kí tự");

        RuleFor(x => x.SummarizeText)
            .MaximumLength(2000)
            .WithMessage("SummarizeText không được nhập quá 2000 kí tự");

        RuleFor(x => x.LessonOutcome)
            .MaximumLength(2000)
            .WithMessage("LessonOutcome không được nhập quá 2000 kí tự");

        RuleFor(x => x.CoveragePercent)
            .InclusiveBetween(0, 100)
            .WithMessage("CoveragePercent phải nằm trong khoảng 0-100")
            .When(x => x.CoveragePercent.HasValue);
    }
}
