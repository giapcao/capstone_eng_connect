using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;

public class UpdateLessonScriptValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonScriptTestData.NormalValidatorCases), MemberType = typeof(UpdateLessonScriptTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonScriptTestData.Definition, caseSet);
    }
}