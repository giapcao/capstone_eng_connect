using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById;

public class GetLessonRecordByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetLessonRecordByIdTestData.HandlerBranchCases), MemberType = typeof(GetLessonRecordByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonRecordByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonRecordByIdTestData.Definition.ValidatorTypeFullName));
    }
}