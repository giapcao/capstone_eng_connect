using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;

public class CreateSupportTicketMessageHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateSupportTicketMessageTestData.NormalHandlerCases), MemberType = typeof(CreateSupportTicketMessageTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateSupportTicketMessageTestData.Definition, caseSet);
    }
}