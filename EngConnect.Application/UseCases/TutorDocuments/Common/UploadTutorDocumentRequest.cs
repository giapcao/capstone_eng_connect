using Microsoft.AspNetCore.Http;

namespace EngConnect.Application.UseCases.TutorDocuments.Common;

public class  UploadTutorDocumentRequest
{
    public string? Name { get; set; }
    public string? DocType { get; set; }
    public string? IssuedBy { get; set; }
    public DateOnly? IssuedAt { get; set; }
    public DateOnly? ExpiredAt { get; set; }
    public string? Status { get; set; }
    public IFormFile? File { get; set; }
    public string? FileName { get; set; }
}
