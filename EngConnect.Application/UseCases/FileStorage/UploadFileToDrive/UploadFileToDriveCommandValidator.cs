using FluentValidation;

namespace EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;

public class UploadFileToDriveCommandValidator : AbstractValidator<UploadFileToDriveCommand>
{
    public UploadFileToDriveCommandValidator()
    {
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.File.FileName).NotEmpty();
        RuleFor(x => x.File.Content).NotNull();
        RuleFor(x => x.File.Length).GreaterThan(0);
        RuleFor(y=>y.Prefix).NotEmpty();
    }
}