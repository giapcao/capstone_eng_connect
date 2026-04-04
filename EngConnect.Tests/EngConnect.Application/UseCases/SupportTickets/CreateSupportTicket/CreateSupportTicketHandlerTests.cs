using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;

public class CreateSupportTicketHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateSupportTicketTestData.NormalHandlerCases), MemberType = typeof(CreateSupportTicketTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateSupportTicketTestData.Definition, caseSet);
    }
}