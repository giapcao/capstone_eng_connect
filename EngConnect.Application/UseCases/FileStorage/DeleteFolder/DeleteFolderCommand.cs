using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.FileStorage.DeleteFolder;

public class DeleteFolderCommand : ICommand
{
    public required string FolderId { get; set; }
}
