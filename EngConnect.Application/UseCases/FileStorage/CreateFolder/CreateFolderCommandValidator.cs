using EngConnect.BuildingBlock.Application.Base;
using FluentValidation;

namespace EngConnect.Application.UseCases.FileStorage.CreateFolder;

public class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
{
    public CreateFolderCommandValidator()
    {
        RuleFor(x => x.FolderName)
            .NotEmpty().WithMessage("Folder name is required")
            .NotNull().WithMessage("Folder name cannot be null")
            .MaximumLength(255).WithMessage("Folder name cannot exceed 255 characters");

        RuleFor(x => x.ParentFolderId)
            .NotEmpty().WithMessage("Parent folder ID is required")
            .NotNull().WithMessage("Parent folder ID cannot be null");
    }
}
