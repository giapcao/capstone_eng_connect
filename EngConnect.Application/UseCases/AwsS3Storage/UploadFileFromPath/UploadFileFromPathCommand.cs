using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.AwsS3Storage.UploadFileFromPath;

public class UploadFileFromPathCommand : ICommand<FileUploadResult>
{
    public required string FilePath { get; set; }
    public required Guid UserId { get; set; }
    public required string Prefix { get; set; }
    public required string ContentType { get; set; }
}