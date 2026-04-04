using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages;

public class GetListSupportTicketMessagesValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListSupportTicketMessagesTestData.Definition.ValidatorTypeFullName));
    }
}