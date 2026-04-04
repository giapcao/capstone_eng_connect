using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;

public class UpdateSupportTicketMessageValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateSupportTicketMessageTestData.NormalValidatorCases), MemberType = typeof(UpdateSupportTicketMessageTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateSupportTicketMessageTestData.Definition, caseSet);
    }
}