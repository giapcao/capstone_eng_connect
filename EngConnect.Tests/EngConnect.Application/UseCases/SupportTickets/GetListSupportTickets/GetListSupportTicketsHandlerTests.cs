using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets;

public class GetListSupportTicketsHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListSupportTicketsTestData.NormalHandlerCases), MemberType = typeof(GetListSupportTicketsTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListSupportTicketsTestData.Definition, caseSet);
    }
}