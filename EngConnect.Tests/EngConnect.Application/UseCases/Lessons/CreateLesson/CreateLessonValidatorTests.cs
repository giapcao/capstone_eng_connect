using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.CreateLesson;

public class CreateLessonValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateLessonTestData.NormalValidatorCases), MemberType = typeof(CreateLessonTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonTestData.Definition, caseSet);
    }
}