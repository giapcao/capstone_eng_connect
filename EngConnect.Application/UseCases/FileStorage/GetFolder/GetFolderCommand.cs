using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.FileStorage.GetFolder;

public class GetFolderCommand : ICommand<string>
{
    public required string FolderName { get; set; }
    
    public required string ParentFolderId { get; set; }
}
