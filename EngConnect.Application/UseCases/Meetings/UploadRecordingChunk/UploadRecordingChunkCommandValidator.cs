using FluentValidation;

namespace EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

public class UploadRecordingChunkCommandValidator : AbstractValidator<UploadRecordingChunkCommand>
{
    public UploadRecordingChunkCommandValidator()
    {
        RuleFor(x => x.LessonId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ChunkIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.FileName).NotEmpty();
        RuleFor(x => x.File.Content).NotNull();
        RuleFor(x => x.File.Length).GreaterThan(0);
    }
}
