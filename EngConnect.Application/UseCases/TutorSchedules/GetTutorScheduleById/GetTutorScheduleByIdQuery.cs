using EngConnect.BuildingBlock.Application.Base;
using EngConnect.Application.UseCases.TutorSchedules.Common;

namespace EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById;

public sealed record GetTutorScheduleByIdQuery(Guid Id) : IQuery<GetTutorScheduleResponse>;