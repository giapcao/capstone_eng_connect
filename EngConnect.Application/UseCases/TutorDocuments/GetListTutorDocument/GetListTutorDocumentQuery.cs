using EngConnect.Application.UseCases.TutorDocuments.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument;

public record GetListTutorDocumentQuery : BaseQuery<PaginationResult<GetTutorDocumentResponse>>
{
    [BindNever]
    public Guid? TutorId { get; set; }
    public string? DocType { get; set; }
    public string? Status { get; set; }
}
