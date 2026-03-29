using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.RegisterUserStaff;

public record RegisterUserStaffCommand : ICommand<RegisterUserStaffResponse>
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
}
