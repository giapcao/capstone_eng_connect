using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Tutors.UpdateTutor
{
    public record UpdateTutorCommand(Guid Id, UpdateTutorRequest Request) : ICommand;
}
