using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest;

public class UpdateLessonRescheduleRequestValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonRescheduleRequestTestData.NormalValidatorCases), MemberType = typeof(UpdateLessonRescheduleRequestTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonRescheduleRequestTestData.Definition, caseSet);
    }
}