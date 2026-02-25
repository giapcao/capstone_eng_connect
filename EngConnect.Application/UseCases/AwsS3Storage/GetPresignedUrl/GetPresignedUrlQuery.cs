using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;

public record GetPresignedUrlQuery : IQuery<string>
{
    public required string FileName { get; set; }
    public int DurationMinutes { get; set; } = 15;
}