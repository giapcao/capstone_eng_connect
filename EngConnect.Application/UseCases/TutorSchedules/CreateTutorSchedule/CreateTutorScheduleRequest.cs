namespace EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule;

public sealed class CreateTutorScheduleRequest
{
    public Guid TutorId { get; set; }
    public string? Weekday { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}