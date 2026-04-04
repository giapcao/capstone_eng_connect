using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;

public class UpdateSupportTicketHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketTestData.NormalHandlerCases), MemberType = typeof(UpdateSupportTicketTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateSupportTicketTestData.Definition, caseSet);
    }
}