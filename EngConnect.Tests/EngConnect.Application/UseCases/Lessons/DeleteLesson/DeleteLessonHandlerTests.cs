using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.DeleteLesson;

public class DeleteLessonHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteLessonTestData.NormalHandlerCases), MemberType = typeof(DeleteLessonTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteLessonTestData.Definition, caseSet);
    }
}