namespace EngConnect.Application.UseCases.TutorDocuments.Common;

public class GetTutorDocumentResponse
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public string? Name { get; set; }
    public string? DocType { get; set; }
    public string Url { get; set; } = null!;
    public string? IssuedBy { get; set; }
    public DateOnly? IssuedAt { get; set; }
    public DateOnly? ExpiredAt { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
