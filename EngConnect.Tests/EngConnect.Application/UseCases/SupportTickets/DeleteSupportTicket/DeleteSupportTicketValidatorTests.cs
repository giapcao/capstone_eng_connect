using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket;

public class DeleteSupportTicketValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteSupportTicketTestData.Definition.ValidatorTypeFullName));
    }
}