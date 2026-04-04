using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;

public class UpdateSupportTicketMessageBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketMessageTestData.HandlerBranchCases), MemberType = typeof(UpdateSupportTicketMessageTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateSupportTicketMessageTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateSupportTicketMessageTestData.ValidatorBranchCases), MemberType = typeof(UpdateSupportTicketMessageTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateSupportTicketMessageTestData.Definition, caseSet);
    }
}