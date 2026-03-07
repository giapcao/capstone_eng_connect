using FluentValidation;

namespace EngConnect.Application.UseCases.AwsS3Storage.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.FileName).NotEmpty();
        RuleFor(x => x.File.Content).NotNull();
        RuleFor(x => x.File.Length).GreaterThan(0);
        RuleFor(x=>x.Prefix).NotEmpty();
    }
}