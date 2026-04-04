using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;

public class CreateSupportTicketBranchTests
{
    [Theory]
    [MemberData(nameof(CreateSupportTicketTestData.HandlerBranchCases), MemberType = typeof(CreateSupportTicketTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateSupportTicketTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateSupportTicketTestData.ValidatorBranchCases), MemberType = typeof(CreateSupportTicketTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateSupportTicketTestData.Definition, caseSet);
    }
}