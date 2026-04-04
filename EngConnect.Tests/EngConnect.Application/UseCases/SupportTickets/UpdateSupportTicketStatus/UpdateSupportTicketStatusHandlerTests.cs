using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;

public class UpdateSupportTicketStatusHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketStatusTestData.NormalHandlerCases), MemberType = typeof(UpdateSupportTicketStatusTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateSupportTicketStatusTestData.Definition, caseSet);
    }
}