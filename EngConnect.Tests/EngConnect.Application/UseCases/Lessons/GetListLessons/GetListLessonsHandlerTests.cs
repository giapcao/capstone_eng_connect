using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetListLessons;

public class GetListLessonsHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListLessonsTestData.NormalHandlerCases), MemberType = typeof(GetListLessonsTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonsTestData.Definition, caseSet);
    }
}