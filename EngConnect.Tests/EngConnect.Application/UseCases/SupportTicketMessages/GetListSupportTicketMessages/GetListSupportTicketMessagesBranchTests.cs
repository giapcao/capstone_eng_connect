using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages;

public class GetListSupportTicketMessagesBranchTests
{
    [Theory]
    [MemberData(nameof(GetListSupportTicketMessagesTestData.HandlerBranchCases), MemberType = typeof(GetListSupportTicketMessagesTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListSupportTicketMessagesTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListSupportTicketMessagesTestData.Definition.ValidatorTypeFullName));
    }
}