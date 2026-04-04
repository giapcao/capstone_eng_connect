using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;

public class CreateLessonScriptBranchTests
{
    [Theory]
    [MemberData(nameof(CreateLessonScriptTestData.HandlerBranchCases), MemberType = typeof(CreateLessonScriptTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonScriptTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateLessonScriptTestData.ValidatorBranchCases), MemberType = typeof(CreateLessonScriptTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonScriptTestData.Definition, caseSet);
    }
}