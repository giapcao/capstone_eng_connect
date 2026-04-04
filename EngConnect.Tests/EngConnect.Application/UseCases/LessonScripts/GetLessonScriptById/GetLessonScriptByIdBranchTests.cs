using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById;

public class GetLessonScriptByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetLessonScriptByIdTestData.HandlerBranchCases), MemberType = typeof(GetLessonScriptByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonScriptByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonScriptByIdTestData.Definition.ValidatorTypeFullName));
    }
}