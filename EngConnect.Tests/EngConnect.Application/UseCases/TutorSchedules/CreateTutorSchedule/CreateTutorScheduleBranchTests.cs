using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule;

public class CreateTutorScheduleBranchTests
{
    [Theory]
    [MemberData(nameof(CreateTutorScheduleTestData.HandlerBranchCases), MemberType = typeof(CreateTutorScheduleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateTutorScheduleTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateTutorScheduleTestData.ValidatorBranchCases), MemberType = typeof(CreateTutorScheduleTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateTutorScheduleTestData.Definition, caseSet);
    }
}