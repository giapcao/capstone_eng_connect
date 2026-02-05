

using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Students.UpdateStatusStudent;

public record UpdateStatusStudentCommand : ICommand
{
    public Guid Id {get;set;}
}