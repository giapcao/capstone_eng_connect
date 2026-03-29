namespace EngConnect.Application.UseCases.Users.Common;

public class UserInfo
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
}
