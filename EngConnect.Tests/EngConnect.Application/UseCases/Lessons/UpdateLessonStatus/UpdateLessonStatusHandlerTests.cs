using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;

public class UpdateLessonStatusHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonStatusTestData.NormalHandlerCases), MemberType = typeof(UpdateLessonStatusTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonStatusTestData.Definition, caseSet);
    }
}