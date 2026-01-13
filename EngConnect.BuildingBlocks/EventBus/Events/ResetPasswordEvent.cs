using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class ResetPasswordEvent : NotificationEvent
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string ResetPasswordToken { get; set; } = null!;

    public static ResetPasswordEvent Create(Guid customerId, string email, string fullName,
        string resetPasswordToken)
    {
        return new ResetPasswordEvent
        {
            IssuerId = customerId,
            ResourceId = customerId.ToString(),
            ResourceType = "Customer",
            Email = email,
            FullName = fullName,
            ResetPasswordToken = resetPasswordToken
        };
    }
}