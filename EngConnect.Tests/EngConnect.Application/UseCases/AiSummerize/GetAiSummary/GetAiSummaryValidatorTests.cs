using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public class GetAiSummaryValidatorTests
{
    [Theory]
    [MemberData(nameof(GetAiSummaryTestData.NormalValidatorCases), MemberType = typeof(GetAiSummaryTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetAiSummaryTestData.Definition, caseSet);
    }
}