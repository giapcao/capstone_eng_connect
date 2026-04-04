using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords;

public class GetListLessonRecordsHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListLessonRecordsTestData.NormalHandlerCases), MemberType = typeof(GetListLessonRecordsTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonRecordsTestData.Definition, caseSet);
    }
}