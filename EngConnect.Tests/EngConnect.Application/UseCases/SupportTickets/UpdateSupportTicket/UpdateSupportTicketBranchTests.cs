using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;

public class UpdateSupportTicketBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketTestData.HandlerBranchCases), MemberType = typeof(UpdateSupportTicketTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateSupportTicketTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateSupportTicketTestData.ValidatorBranchCases), MemberType = typeof(UpdateSupportTicketTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateSupportTicketTestData.Definition, caseSet);
    }
}