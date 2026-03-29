using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;

public class UpdateIntroVideoUrlTutorCommand : ICommand
{
    public required FileUpload File { get; set; }
    public required Guid Id { get; set; }
}
