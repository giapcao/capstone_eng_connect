using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById;

public class GetTutorScheduleByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetTutorScheduleByIdTestData.NormalHandlerCases), MemberType = typeof(GetTutorScheduleByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorScheduleByIdTestData.Definition, caseSet);
    }
}