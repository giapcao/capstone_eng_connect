using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

public interface IWhisperApiService
{
    Task<string?> Transcribe(FileUpload fileUpload);
}