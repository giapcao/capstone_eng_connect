using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages;

public class GetListSupportTicketMessagesHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListSupportTicketMessagesTestData.NormalHandlerCases), MemberType = typeof(GetListSupportTicketMessagesTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListSupportTicketMessagesTestData.Definition, caseSet);
    }
}