using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.Application.UseCases.TutorSchedules.Common;

namespace EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule;

public record GetListTutorScheduleQuery(Guid? TutorId, string? Weekday) : BaseQuery<PaginationResult<GetTutorScheduleResponse>>;