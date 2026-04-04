using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;

public class UpdateLessonScriptBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonScriptTestData.HandlerBranchCases), MemberType = typeof(UpdateLessonScriptTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonScriptTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateLessonScriptTestData.ValidatorBranchCases), MemberType = typeof(UpdateLessonScriptTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonScriptTestData.Definition, caseSet);
    }
}