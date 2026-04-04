using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Meetings.EndMeeting;

public record EndMeetingCommand(Guid LessonId, Guid UserId, int? TotalChunks = null) : ICommand;
