using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Students.DeleteStudent;

public record DeleteStudentCommand : ICommand
{
    public Guid Id { get; set; }
    
    public Guid UserId {get; set; }
}