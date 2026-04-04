using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;

public class UpdateSupportTicketValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketTestData.NormalValidatorCases), MemberType = typeof(UpdateSupportTicketTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateSupportTicketTestData.Definition, caseSet);
    }
}