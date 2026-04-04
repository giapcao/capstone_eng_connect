using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest;

public class CreateLessonRescheduleRequestValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateLessonRescheduleRequestTestData.NormalValidatorCases), MemberType = typeof(CreateLessonRescheduleRequestTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonRescheduleRequestTestData.Definition, caseSet);
    }
}