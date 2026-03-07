using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;

public class GetFileFromDriveCommand : ICommand<FileUploadResult>
{
    public required string FileId { get; set; }
}