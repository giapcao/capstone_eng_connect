using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;

public record CheckFileExistsQuery : IQuery<bool>
{
    public required string FileName { get; set; }
}