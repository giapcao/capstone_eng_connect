using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.CreateLesson;

public class CreateLessonBranchTests
{
    [Theory]
    [MemberData(nameof(CreateLessonTestData.HandlerBranchCases), MemberType = typeof(CreateLessonTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateLessonTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateLessonTestData.ValidatorBranchCases), MemberType = typeof(CreateLessonTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateLessonTestData.Definition, caseSet);
    }
}