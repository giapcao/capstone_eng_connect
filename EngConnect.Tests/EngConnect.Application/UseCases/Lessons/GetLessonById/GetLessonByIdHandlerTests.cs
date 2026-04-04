using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetLessonById;

public class GetLessonByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetLessonByIdTestData.NormalHandlerCases), MemberType = typeof(GetLessonByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonByIdTestData.Definition, caseSet);
    }
}