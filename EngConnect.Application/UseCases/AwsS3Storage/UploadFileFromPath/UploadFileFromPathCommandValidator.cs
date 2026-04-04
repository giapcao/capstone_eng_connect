using FluentValidation;

namespace EngConnect.Application.UseCases.AwsS3Storage.UploadFileFromPath;

public class UploadFileFromPathCommandValidator : AbstractValidator<UploadFileFromPathCommand>
{
    public UploadFileFromPathCommandValidator()
    {
        RuleFor(x => x.FilePath).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Prefix).NotEmpty();
        RuleFor(x => x.ContentType).NotEmpty();
    }
}