using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest;

public class CreateLessonRescheduleRequestHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateLessonRescheduleRequestTestData.NormalHandlerCases), MemberType = typeof(CreateLessonRescheduleRequestTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonRescheduleRequestTestData.Definition, caseSet);
    }
}