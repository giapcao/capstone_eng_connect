using EngConnect.Application.UseCases.TutorDocuments.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorDocuments.GetTutorDocumentById;

public record GetTutorDocumentByIdQuery(Guid Id) : IQuery<GetTutorDocumentResponse>;
