using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;

public class UpdateLessonRecordValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonRecordTestData.NormalValidatorCases), MemberType = typeof(UpdateLessonRecordTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonRecordTestData.Definition, caseSet);
    }
}