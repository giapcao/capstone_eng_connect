using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule;

public record DeleteTutorScheduleCommand(Guid Id) : ICommand;