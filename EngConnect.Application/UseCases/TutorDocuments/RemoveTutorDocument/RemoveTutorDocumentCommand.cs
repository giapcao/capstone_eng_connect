using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;

public class RemoveTutorDocumentCommand : ICommand
{
    public Guid DocumentId { get; set; }
    public Guid TutorId { get; set; }
}
