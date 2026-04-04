using EngConnect.BuildingBlock.Application.Validation.Validation;
using FluentValidation;

namespace EngConnect.Application.UseCases.Lessons.CreateLesson;

public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand>
{
    public CreateLessonCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("MÃ£ há»c sinh (StudentId) khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng.");

        RuleFor(x => x.EnrollmentId)
            .NotEmpty().WithMessage("MÃ£ Ä‘Äƒng kÃ½ (EnrollmentId) khÃ´ng Ä‘Æ°á»£c Ä‘á»ƒ trá»‘ng.");

        RuleFor(x => x)
            .Must(x => x.ModuleId.HasValue == x.SessionId.HasValue)
            .WithMessage("ModuleId vÃ  SessionId pháº£i cÃ¹ng cÃ³ giÃ¡ trá»‹ hoáº·c cÃ¹ng Ä‘á»ƒ trá»‘ng.");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Thá»i gian báº¯t Ä‘áº§u pháº£i á»Ÿ trong tÆ°Æ¡ng lai.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("Thá»i gian káº¿t thÃºc pháº£i sau thá»i gian báº¯t Ä‘áº§u.");
    }
}
