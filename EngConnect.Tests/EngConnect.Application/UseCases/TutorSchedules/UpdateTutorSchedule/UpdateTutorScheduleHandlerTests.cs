using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

public class UpdateTutorScheduleHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateTutorScheduleTestData.NormalHandlerCases), MemberType = typeof(UpdateTutorScheduleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateTutorScheduleTestData.Definition, caseSet);
    }
}