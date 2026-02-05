using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;

public class DeleteFileFromDriveCommand : ICommand
{
    public required string FileId { get; set; }
}