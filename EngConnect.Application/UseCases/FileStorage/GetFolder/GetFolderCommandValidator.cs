using EngConnect.BuildingBlock.Application.Base;
using FluentValidation;

namespace EngConnect.Application.UseCases.FileStorage.GetFolder;

public class GetFolderCommandValidator : AbstractValidator<GetFolderCommand>
{
    public GetFolderCommandValidator()
    {
        RuleFor(x => x.FolderName)
            .NotEmpty().WithMessage("Folder name is required")
            .NotNull().WithMessage("Folder name cannot be null");

        RuleFor(x => x.ParentFolderId)
            .NotEmpty().WithMessage("Parent folder ID is required")
            .NotNull().WithMessage("Parent folder ID cannot be null");
    }
}
