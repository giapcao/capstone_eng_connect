using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLesson;

public class UpdateLessonHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonTestData.NormalHandlerCases), MemberType = typeof(UpdateLessonTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonTestData.Definition, caseSet);
    }
}