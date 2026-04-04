using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById;

public class GetSupportTicketByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetSupportTicketByIdTestData.NormalHandlerCases), MemberType = typeof(GetSupportTicketByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetSupportTicketByIdTestData.Definition, caseSet);
    }
}