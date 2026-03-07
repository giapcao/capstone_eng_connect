using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.FileStorage.CreateFolder;

public class CreateFolderCommand : ICommand<string>
{
    public required string FolderName { get; set; }
    
    public required string ParentFolderId { get; set; }
}
