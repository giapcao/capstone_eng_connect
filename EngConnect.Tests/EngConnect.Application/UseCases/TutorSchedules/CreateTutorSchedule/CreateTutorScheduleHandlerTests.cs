using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule;

public class CreateTutorScheduleHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateTutorScheduleTestData.NormalHandlerCases), MemberType = typeof(CreateTutorScheduleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateTutorScheduleTestData.Definition, caseSet);
    }
}