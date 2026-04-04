using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;

public class CreateLessonRecordHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateLessonRecordTestData.NormalHandlerCases), MemberType = typeof(CreateLessonRecordTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonRecordTestData.Definition, caseSet);
    }
}