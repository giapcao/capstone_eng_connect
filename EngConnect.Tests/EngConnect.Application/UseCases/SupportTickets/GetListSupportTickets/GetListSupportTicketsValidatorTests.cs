using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets;

public class GetListSupportTicketsValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListSupportTicketsTestData.Definition.ValidatorTypeFullName));
    }
}