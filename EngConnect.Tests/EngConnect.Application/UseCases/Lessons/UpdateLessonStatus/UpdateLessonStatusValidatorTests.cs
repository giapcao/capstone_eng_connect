using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;

public class UpdateLessonStatusValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonStatusTestData.NormalValidatorCases), MemberType = typeof(UpdateLessonStatusTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonStatusTestData.Definition, caseSet);
    }
}