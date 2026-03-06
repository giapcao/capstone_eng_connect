using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.RegisterStudent;

public class RegisterStudentCommand : ICommand
{
    public Guid UserId {get;set;}
    
    public string? Notes { get; set; }

    public string? School { get; set; }

    public string? Grade { get; set; }

    public string? Class { get; set; }
}
