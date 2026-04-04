using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;

public class UpdateSupportTicketStatusBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketStatusTestData.HandlerBranchCases), MemberType = typeof(UpdateSupportTicketStatusTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateSupportTicketStatusTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateSupportTicketStatusTestData.ValidatorBranchCases), MemberType = typeof(UpdateSupportTicketStatusTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateSupportTicketStatusTestData.Definition, caseSet);
    }
}