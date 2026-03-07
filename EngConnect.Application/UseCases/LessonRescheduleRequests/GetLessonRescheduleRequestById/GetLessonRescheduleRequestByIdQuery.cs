using EngConnect.BuildingBlock.Application.Base;
using EngConnect.Application.UseCases.LessonRescheduleRequests.Common;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById;

public sealed record GetLessonRescheduleRequestByIdQuery(Guid Id) : IQuery<GetRescheduleRequestResponse>;