using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule;

public class DeleteTutorScheduleValidatorTests
{
    [Theory]
    [MemberData(nameof(DeleteTutorScheduleTestData.NormalValidatorCases), MemberType = typeof(DeleteTutorScheduleTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteTutorScheduleTestData.Definition, caseSet);
    }
}