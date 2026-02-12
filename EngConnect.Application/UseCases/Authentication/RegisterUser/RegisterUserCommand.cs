using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.RegisterUser;

public record RegisterUserCommand: ICommand
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public string? Phone { get; set; }
    public string? AddressNum { get; set; }
    public string? ProvinceId { get; set; }
    public string? ProvinceName { get; set; }
    public string? WardId { get; set; }
    public string? WardName { get; set; }
    public required string Password { get; set; }
    
    //260212: Add role for check register with student or tutor
    public required string Role { get; set; }
}