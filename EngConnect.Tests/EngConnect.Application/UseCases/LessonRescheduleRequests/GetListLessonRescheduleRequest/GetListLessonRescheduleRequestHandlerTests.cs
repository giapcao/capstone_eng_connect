using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest;

public class GetListLessonRescheduleRequestHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListLessonRescheduleRequestTestData.NormalHandlerCases), MemberType = typeof(GetListLessonRescheduleRequestTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonRescheduleRequestTestData.Definition, caseSet);
    }
}