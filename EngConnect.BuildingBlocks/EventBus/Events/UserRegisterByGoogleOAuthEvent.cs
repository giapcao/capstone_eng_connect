using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

/// <summary>
/// Event published when a user registers by Google
/// </summary>
public class UserRegisterByGoogleOAuthEvent: NotificationEvent
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string GeneratedPassword { get; set; } = null!;
    
    public static UserRegisterByGoogleOAuthEvent Create(Guid userId, string email, string fullName, string generatedPassword)
    {
        return new UserRegisterByGoogleOAuthEvent
        {
            IssuerId = userId,
            ResourceId = userId.ToString(),
            ResourceType = nameof(EventResourceType.User),
            Email = email,
            FullName = fullName,
            GeneratedPassword = generatedPassword
        };
    }
}