using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.CreateLesson;

public class CreateLessonHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateLessonTestData.NormalHandlerCases), MemberType = typeof(CreateLessonTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonTestData.Definition, caseSet);
    }
}