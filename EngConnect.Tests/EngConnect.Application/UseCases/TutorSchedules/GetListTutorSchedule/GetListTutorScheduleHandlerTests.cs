using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule;

public class GetListTutorScheduleHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListTutorScheduleTestData.NormalHandlerCases), MemberType = typeof(GetListTutorScheduleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorScheduleTestData.Definition, caseSet);
    }
}