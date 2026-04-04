using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById;

public class GetSupportTicketByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetSupportTicketByIdTestData.HandlerBranchCases), MemberType = typeof(GetSupportTicketByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetSupportTicketByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetSupportTicketByIdTestData.Definition.ValidatorTypeFullName));
    }
}