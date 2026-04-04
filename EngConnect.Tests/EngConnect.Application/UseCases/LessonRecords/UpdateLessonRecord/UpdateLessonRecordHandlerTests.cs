using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;

public class UpdateLessonRecordHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonRecordTestData.NormalHandlerCases), MemberType = typeof(UpdateLessonRecordTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonRecordTestData.Definition, caseSet);
    }
}