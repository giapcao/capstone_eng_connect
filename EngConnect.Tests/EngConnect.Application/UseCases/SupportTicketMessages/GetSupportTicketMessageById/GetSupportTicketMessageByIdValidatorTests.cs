using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById;

public class GetSupportTicketMessageByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetSupportTicketMessageByIdTestData.Definition.ValidatorTypeFullName));
    }
}