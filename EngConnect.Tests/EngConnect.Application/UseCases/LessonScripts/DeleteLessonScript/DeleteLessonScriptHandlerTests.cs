using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript;

public class DeleteLessonScriptHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteLessonScriptTestData.NormalHandlerCases), MemberType = typeof(DeleteLessonScriptTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteLessonScriptTestData.Definition, caseSet);
    }
}