using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket;

public class DeleteSupportTicketHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteSupportTicketTestData.NormalHandlerCases), MemberType = typeof(DeleteSupportTicketTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteSupportTicketTestData.Definition, caseSet);
    }
}