using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket;

public class DeleteSupportTicketBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteSupportTicketTestData.HandlerBranchCases), MemberType = typeof(DeleteSupportTicketTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteSupportTicketTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteSupportTicketTestData.Definition.ValidatorTypeFullName));
    }
}