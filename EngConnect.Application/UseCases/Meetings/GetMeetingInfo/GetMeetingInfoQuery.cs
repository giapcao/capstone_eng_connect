using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Meetings.GetMeetingInfo;

public record GetMeetingInfoQuery(Guid LessonId, Guid UserId) : IQuery<GetMeetingInfoResponse>;