using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;

public record DownloadFileQuery : IQuery<FileReadResult>
{
    public required string FileName { get; set; }
}