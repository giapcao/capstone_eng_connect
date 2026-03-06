using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.Application.UseCases.LessonRescheduleRequests.Common;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest;

public sealed record GetListLessonRescheduleRequestQuery(Guid? LessonId, Guid? StudentId, string? Status) : BaseQuery<PaginationResult<GetRescheduleRequestResponse>>;