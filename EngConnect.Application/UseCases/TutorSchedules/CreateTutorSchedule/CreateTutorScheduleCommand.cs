using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule;

public record CreateTutorScheduleCommand(CreateTutorScheduleRequest Request) : ICommand;