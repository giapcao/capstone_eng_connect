using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLesson;

public class UpdateLessonValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonTestData.NormalValidatorCases), MemberType = typeof(UpdateLessonTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonTestData.Definition, caseSet);
    }
}