using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Tutors.DeleteTutor
{
    public record DeleteTutorCommand(Guid Id) : ICommand;
}
