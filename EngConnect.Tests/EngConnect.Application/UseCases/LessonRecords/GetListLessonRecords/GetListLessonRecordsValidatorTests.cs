using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords;

public class GetListLessonRecordsValidatorTests
{
    [Theory]
    [MemberData(nameof(GetListLessonRecordsTestData.NormalValidatorCases), MemberType = typeof(GetListLessonRecordsTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListLessonRecordsTestData.Definition, caseSet);
    }
}