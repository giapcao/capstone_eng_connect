using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById;

public class GetTutorScheduleByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetTutorScheduleByIdTestData.HandlerBranchCases), MemberType = typeof(GetTutorScheduleByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorScheduleByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorScheduleByIdTestData.Definition.ValidatorTypeFullName));
    }
}