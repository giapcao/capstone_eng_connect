using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;

public class GetListLessonScriptsHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListLessonScriptsTestData.NormalHandlerCases), MemberType = typeof(GetListLessonScriptsTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonScriptsTestData.Definition, caseSet);
    }
}