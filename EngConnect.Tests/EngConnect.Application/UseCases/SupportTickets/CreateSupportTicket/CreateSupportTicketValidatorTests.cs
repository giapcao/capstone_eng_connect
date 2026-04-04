using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;

public class CreateSupportTicketValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateSupportTicketTestData.NormalValidatorCases), MemberType = typeof(CreateSupportTicketTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateSupportTicketTestData.Definition, caseSet);
    }
}