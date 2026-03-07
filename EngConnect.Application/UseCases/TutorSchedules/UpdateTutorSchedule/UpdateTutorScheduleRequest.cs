namespace EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

public sealed class UpdateTutorScheduleRequest
{
    public Guid Id { get; set; }
    public string? Weekday { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}