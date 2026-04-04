using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById;

public class GetSupportTicketByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetSupportTicketByIdTestData.Definition.ValidatorTypeFullName));
    }
}