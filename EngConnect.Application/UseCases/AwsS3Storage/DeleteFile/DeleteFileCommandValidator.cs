using FluentValidation;

namespace EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;

public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
{
    public DeleteFileCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
    }
}