using EngConnect.BuildingBlock.Application.Base;
using FluentValidation;

namespace EngConnect.Application.UseCases.FileStorage.DeleteFolder;

public class DeleteFolderCommandValidator : AbstractValidator<DeleteFolderCommand>
{
    public DeleteFolderCommandValidator()
    {
        RuleFor(x => x.FolderId)
            .NotEmpty().WithMessage("Folder ID is required")
            .NotNull().WithMessage("Folder ID cannot be null");
    }
}
