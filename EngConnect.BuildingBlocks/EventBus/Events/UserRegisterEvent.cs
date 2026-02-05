using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class UserRegisterEvent : NotificationEvent
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string VerificationToken { get; set; } = null!;

    public static UserRegisterEvent Create(Guid customerId, string email, string fullName,
        string verificationToken)
    {
        return new UserRegisterEvent
        {
            IssuerId = customerId,
            ResourceId = customerId.ToString(),
            ResourceType = "Customer",
            Email = email,
            FullName = fullName,
            VerificationToken = verificationToken
        };
    }
}