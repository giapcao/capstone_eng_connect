using FluentValidation;

namespace EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;

public class CreateLessonRecordCommandValidator : AbstractValidator<CreateLessonRecordCommand>
{
    public CreateLessonRecordCommandValidator()
    {
        RuleFor(x => x.LessonId)
            .NotEmpty()
            .WithMessage("LessonId không được để trống");

        RuleFor(x => x.RecordUrl)
            .NotEmpty()
            .WithMessage("RecordUrl không được để trống")
            .MaximumLength(500)
            .WithMessage("RecordUrl không được quá 500 ký tự");

        RuleFor(x => x.DurationSeconds)
            .GreaterThan(0)
            .When(x => x.DurationSeconds.HasValue)
            .WithMessage("DurationSeconds phải lớn hơn 0");
        
        RuleFor(x => x.RecordingStartedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.RecordingStartedAt.HasValue)
            .WithMessage("Thời gian bắt đầu không được ở tương lai");
        
        RuleFor(x => x.RecordingEndedAt)
            .GreaterThan(x => x.RecordingStartedAt)
            .When(x => x.RecordingStartedAt.HasValue && x.RecordingEndedAt.HasValue)
            .WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");
    }
}
