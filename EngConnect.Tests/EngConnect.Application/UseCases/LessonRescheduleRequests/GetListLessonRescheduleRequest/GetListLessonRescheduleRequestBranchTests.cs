using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest;

public class GetListLessonRescheduleRequestBranchTests
{
    [Theory]
    [MemberData(nameof(GetListLessonRescheduleRequestTestData.HandlerBranchCases), MemberType = typeof(GetListLessonRescheduleRequestTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonRescheduleRequestTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListLessonRescheduleRequestTestData.Definition.ValidatorTypeFullName));
    }
}