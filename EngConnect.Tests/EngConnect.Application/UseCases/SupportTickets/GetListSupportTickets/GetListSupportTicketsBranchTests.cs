using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets;

public class GetListSupportTicketsBranchTests
{
    [Theory]
    [MemberData(nameof(GetListSupportTicketsTestData.HandlerBranchCases), MemberType = typeof(GetListSupportTicketsTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListSupportTicketsTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListSupportTicketsTestData.Definition.ValidatorTypeFullName));
    }
}