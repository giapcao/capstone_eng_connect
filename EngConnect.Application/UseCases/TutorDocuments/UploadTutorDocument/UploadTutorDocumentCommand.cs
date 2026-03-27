using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

public class UploadTutorDocumentCommand : ICommand
{
    public Guid TutorId { get; set; }
    public string? Name { get; set; }
    public string? DocType { get; set; }
    public string? IssuedBy { get; set; }
    public DateOnly? IssuedAt { get; set; }
    public DateOnly? ExpiredAt { get; set; }
    public string? Status { get; set; }
    public required FileUpload File { get; set; }
}
