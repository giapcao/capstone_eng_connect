using EngConnect.BuildingBlock.Contracts.Models.Email;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IEmailService
{
    Task SendEmailAsync(List<string> to, List<string> cc, EmailContent content);
}