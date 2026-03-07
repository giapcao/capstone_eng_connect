using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.AwsS3Storage.UploadFile;

public class UploadFileCommand : ICommand<FileUploadResult>
{
    public required FileUpload File { get; set; }
    public required Guid UserId{get;set;} 
    public required string  Prefix{get;set;}
}