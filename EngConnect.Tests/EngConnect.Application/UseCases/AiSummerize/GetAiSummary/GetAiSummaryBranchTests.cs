using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public class GetAiSummaryBranchTests
{
    [Theory]
    [MemberData(nameof(GetAiSummaryTestData.HandlerBranchCases), MemberType = typeof(GetAiSummaryTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetAiSummaryTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetAiSummaryTestData.ValidatorBranchCases), MemberType = typeof(GetAiSummaryTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetAiSummaryTestData.Definition, caseSet);
    }
}