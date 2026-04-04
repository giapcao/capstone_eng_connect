using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;

public class CreateLessonScriptValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateLessonScriptTestData.NormalValidatorCases), MemberType = typeof(CreateLessonScriptTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonScriptTestData.Definition, caseSet);
    }
}