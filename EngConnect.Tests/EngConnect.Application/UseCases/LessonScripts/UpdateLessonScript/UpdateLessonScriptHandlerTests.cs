using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;

public class UpdateLessonScriptHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonScriptTestData.NormalHandlerCases), MemberType = typeof(UpdateLessonScriptTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonScriptTestData.Definition, caseSet);
    }
}