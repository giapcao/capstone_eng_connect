using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule;

public class GetListTutorScheduleBranchTests
{
    [Theory]
    [MemberData(nameof(GetListTutorScheduleTestData.HandlerBranchCases), MemberType = typeof(GetListTutorScheduleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorScheduleTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorScheduleTestData.Definition.ValidatorTypeFullName));
    }
}