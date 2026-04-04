using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.DeleteSupportTicketMessage;

public class DeleteSupportTicketMessageBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteSupportTicketMessageTestData.HandlerBranchCases), MemberType = typeof(DeleteSupportTicketMessageTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteSupportTicketMessageTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteSupportTicketMessageTestData.Definition.ValidatorTypeFullName));
    }
}