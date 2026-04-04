using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById;

public class GetLessonRescheduleRequestByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetLessonRescheduleRequestByIdTestData.NormalHandlerCases), MemberType = typeof(GetLessonRescheduleRequestByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonRescheduleRequestByIdTestData.Definition, caseSet);
    }
}