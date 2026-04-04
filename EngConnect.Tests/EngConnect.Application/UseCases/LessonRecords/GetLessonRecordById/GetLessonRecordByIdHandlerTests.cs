using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById;

public class GetLessonRecordByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetLessonRecordByIdTestData.NormalHandlerCases), MemberType = typeof(GetLessonRecordByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonRecordByIdTestData.Definition, caseSet);
    }
}