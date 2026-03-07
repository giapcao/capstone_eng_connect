using FluentValidation;

namespace EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;

public class DeleteFileFromDriveCommandValidator : AbstractValidator<DeleteFileFromDriveCommand>
{
    public DeleteFileFromDriveCommandValidator()
    {
        RuleFor(x => x.FileId).NotEmpty();
    }
}