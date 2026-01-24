using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Students.UpdateStudent;

public class UpdateStudentCommand :  ICommand
{
    public Guid Id {get;set;}
    
    public Guid UserId {get;set;}
    
    public string? Notes { get; set; }

    public string? School { get; set; }

    public string? Grade { get; set; }

    public string? Class { get; set; }
}