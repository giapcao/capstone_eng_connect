using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public class GetAiSummaryHandlerTests
{
    [Theory]
    [MemberData(nameof(GetAiSummaryTestData.NormalHandlerCases), MemberType = typeof(GetAiSummaryTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetAiSummaryTestData.Definition, caseSet);
    }
}