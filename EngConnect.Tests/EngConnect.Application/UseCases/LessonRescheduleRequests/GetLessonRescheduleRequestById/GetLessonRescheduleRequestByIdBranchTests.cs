using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById;

public class GetLessonRescheduleRequestByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetLessonRescheduleRequestByIdTestData.HandlerBranchCases), MemberType = typeof(GetLessonRescheduleRequestByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonRescheduleRequestByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonRescheduleRequestByIdTestData.Definition.ValidatorTypeFullName));
    }
}