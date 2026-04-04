using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById;

public class GetSupportTicketMessageByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetSupportTicketMessageByIdTestData.HandlerBranchCases), MemberType = typeof(GetSupportTicketMessageByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetSupportTicketMessageByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetSupportTicketMessageByIdTestData.Definition.ValidatorTypeFullName));
    }
}