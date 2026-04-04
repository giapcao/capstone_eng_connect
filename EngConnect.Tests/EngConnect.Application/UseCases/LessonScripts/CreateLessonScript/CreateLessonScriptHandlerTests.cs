using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;

public class CreateLessonScriptHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateLessonScriptTestData.NormalHandlerCases), MemberType = typeof(CreateLessonScriptTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonScriptTestData.Definition, caseSet);
    }
}