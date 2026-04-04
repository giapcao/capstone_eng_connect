using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.DeleteSupportTicketMessage;

public class DeleteSupportTicketMessageValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteSupportTicketMessageTestData.Definition.ValidatorTypeFullName));
    }
}