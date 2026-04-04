using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;

public class CreateSupportTicketMessageValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateSupportTicketMessageTestData.NormalValidatorCases), MemberType = typeof(CreateSupportTicketMessageTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateSupportTicketMessageTestData.Definition, caseSet);
    }
}