using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord;

public class DeleteLessonRecordHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteLessonRecordTestData.NormalHandlerCases), MemberType = typeof(DeleteLessonRecordTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteLessonRecordTestData.Definition, caseSet);
    }
}