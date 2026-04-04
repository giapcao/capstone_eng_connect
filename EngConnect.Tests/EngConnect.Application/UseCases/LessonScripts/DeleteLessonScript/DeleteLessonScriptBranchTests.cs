using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript;

public class DeleteLessonScriptBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteLessonScriptTestData.HandlerBranchCases), MemberType = typeof(DeleteLessonScriptTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteLessonScriptTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteLessonScriptTestData.Definition.ValidatorTypeFullName));
    }
}