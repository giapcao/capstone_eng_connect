using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById;

public class GetLessonScriptByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetLessonScriptByIdTestData.NormalHandlerCases), MemberType = typeof(GetLessonScriptByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonScriptByIdTestData.Definition, caseSet);
    }
}