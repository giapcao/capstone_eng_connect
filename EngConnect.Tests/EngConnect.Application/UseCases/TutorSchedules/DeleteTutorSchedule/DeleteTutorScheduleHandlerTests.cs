using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule;

public class DeleteTutorScheduleHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteTutorScheduleTestData.NormalHandlerCases), MemberType = typeof(DeleteTutorScheduleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteTutorScheduleTestData.Definition, caseSet);
    }
}