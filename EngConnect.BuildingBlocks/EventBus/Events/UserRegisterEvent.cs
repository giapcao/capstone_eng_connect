using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class UserRegisterEvent : NotificationEvent
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string VerificationToken { get; set; } = null!;

    public static UserRegisterEvent Create(Guid userId, string email, string fullName,
        string verificationToken)
    {
        return new UserRegisterEvent
        {
            IssuerId = userId,
            ResourceId = userId.ToString(),
            ResourceType = nameof(EventResourceType.User),
            Email = email,
            FullName = fullName,
            VerificationToken = verificationToken
        };
    }
}