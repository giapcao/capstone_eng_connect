using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;

public class UpdateSupportTicketStatusValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketStatusTestData.NormalValidatorCases), MemberType = typeof(UpdateSupportTicketStatusTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateSupportTicketStatusTestData.Definition, caseSet);
    }
}