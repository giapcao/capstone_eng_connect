using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;

public record DeleteFileCommand : ICommand<bool>
{
    public required string FileName { get; init; }
}