using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;

public class UpdateSupportTicketMessageHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketMessageTestData.NormalHandlerCases), MemberType = typeof(UpdateSupportTicketMessageTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateSupportTicketMessageTestData.Definition, caseSet);
    }
}