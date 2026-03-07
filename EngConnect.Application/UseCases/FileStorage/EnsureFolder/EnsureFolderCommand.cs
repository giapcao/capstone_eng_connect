using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.FileStorage.EnsureFolder;

public class EnsureFolderCommand : ICommand<string>
{
    public required string FolderName { get; set; }
    
    public required string ParentFolderId { get; set; }
}
