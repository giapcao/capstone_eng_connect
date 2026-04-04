using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest;

public class UpdateLessonRescheduleRequestHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonRescheduleRequestTestData.NormalHandlerCases), MemberType = typeof(UpdateLessonRescheduleRequestTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonRescheduleRequestTestData.Definition, caseSet);
    }
}