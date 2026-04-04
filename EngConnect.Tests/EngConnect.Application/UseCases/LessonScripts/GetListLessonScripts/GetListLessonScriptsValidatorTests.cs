using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;

public class GetListLessonScriptsValidatorTests
{
    [Theory]
    [MemberData(nameof(GetListLessonScriptsTestData.NormalValidatorCases), MemberType = typeof(GetListLessonScriptsTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListLessonScriptsTestData.Definition, caseSet);
    }
}