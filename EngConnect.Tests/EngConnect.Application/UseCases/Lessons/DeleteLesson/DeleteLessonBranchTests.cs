using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.DeleteLesson;

public class DeleteLessonBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteLessonTestData.HandlerBranchCases), MemberType = typeof(DeleteLessonTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteLessonTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteLessonTestData.Definition.ValidatorTypeFullName));
    }
}