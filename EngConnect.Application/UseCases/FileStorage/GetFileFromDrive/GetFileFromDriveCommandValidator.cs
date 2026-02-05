using FluentValidation;

namespace EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;

public class GetFileFromDriveCommandValidator : AbstractValidator<GetFileFromDriveCommand>
{
    public GetFileFromDriveCommandValidator()
    {
        RuleFor(x => x.FileId).NotEmpty();
    }
}