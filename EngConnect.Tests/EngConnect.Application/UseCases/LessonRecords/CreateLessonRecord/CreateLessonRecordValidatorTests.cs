using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;

public class CreateLessonRecordValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateLessonRecordTestData.NormalValidatorCases), MemberType = typeof(CreateLessonRecordTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonRecordTestData.Definition, caseSet);
    }
}