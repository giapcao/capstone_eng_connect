using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.DeleteSupportTicketMessage;

public class DeleteSupportTicketMessageHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteSupportTicketMessageTestData.NormalHandlerCases), MemberType = typeof(DeleteSupportTicketMessageTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteSupportTicketMessageTestData.Definition, caseSet);
    }
}