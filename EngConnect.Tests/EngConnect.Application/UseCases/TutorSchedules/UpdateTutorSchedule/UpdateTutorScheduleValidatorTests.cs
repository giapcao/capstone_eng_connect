using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

public class UpdateTutorScheduleValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateTutorScheduleTestData.NormalValidatorCases), MemberType = typeof(UpdateTutorScheduleTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateTutorScheduleTestData.Definition, caseSet);
    }
}