using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule;

public class DeleteTutorScheduleBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteTutorScheduleTestData.HandlerBranchCases), MemberType = typeof(DeleteTutorScheduleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteTutorScheduleTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(DeleteTutorScheduleTestData.ValidatorBranchCases), MemberType = typeof(DeleteTutorScheduleTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteTutorScheduleTestData.Definition, caseSet);
    }
}