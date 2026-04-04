using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord;

public class DeleteLessonRecordBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteLessonRecordTestData.HandlerBranchCases), MemberType = typeof(DeleteLessonRecordTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteLessonRecordTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteLessonRecordTestData.Definition.ValidatorTypeFullName));
    }
}