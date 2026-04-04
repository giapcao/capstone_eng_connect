using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

public class UpdateTutorScheduleBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateTutorScheduleTestData.HandlerBranchCases), MemberType = typeof(UpdateTutorScheduleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateTutorScheduleTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateTutorScheduleTestData.ValidatorBranchCases), MemberType = typeof(UpdateTutorScheduleTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateTutorScheduleTestData.Definition, caseSet);
    }
}