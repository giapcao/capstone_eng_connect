using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById;

public class GetSupportTicketMessageByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetSupportTicketMessageByIdTestData.NormalHandlerCases), MemberType = typeof(GetSupportTicketMessageByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetSupportTicketMessageByIdTestData.Definition, caseSet);
    }
}