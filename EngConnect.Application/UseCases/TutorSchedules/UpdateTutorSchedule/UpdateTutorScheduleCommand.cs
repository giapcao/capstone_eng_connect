using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

public record UpdateTutorScheduleCommand(UpdateTutorScheduleRequest Request) : ICommand;