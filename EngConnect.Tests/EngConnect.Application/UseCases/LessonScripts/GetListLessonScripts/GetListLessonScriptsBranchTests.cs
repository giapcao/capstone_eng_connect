using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;

public class GetListLessonScriptsBranchTests
{
    [Theory]
    [MemberData(nameof(GetListLessonScriptsTestData.HandlerBranchCases), MemberType = typeof(GetListLessonScriptsTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonScriptsTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetListLessonScriptsTestData.ValidatorBranchCases), MemberType = typeof(GetListLessonScriptsTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListLessonScriptsTestData.Definition, caseSet);
    }
}