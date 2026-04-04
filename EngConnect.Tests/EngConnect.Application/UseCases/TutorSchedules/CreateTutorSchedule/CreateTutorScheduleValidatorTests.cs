using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule;

public class CreateTutorScheduleValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateTutorScheduleTestData.NormalValidatorCases), MemberType = typeof(CreateTutorScheduleTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateTutorScheduleTestData.Definition, caseSet);
    }
}