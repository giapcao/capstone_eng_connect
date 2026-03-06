using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Users.CreateUser;

public class CreateUserCommand: ICommand
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}