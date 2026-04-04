using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;

public class CreateSupportTicketMessageBranchTests
{
    [Theory]
    [MemberData(nameof(CreateSupportTicketMessageTestData.HandlerBranchCases), MemberType = typeof(CreateSupportTicketMessageTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateSupportTicketMessageTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateSupportTicketMessageTestData.ValidatorBranchCases), MemberType = typeof(CreateSupportTicketMessageTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateSupportTicketMessageTestData.Definition, caseSet);
    }
}