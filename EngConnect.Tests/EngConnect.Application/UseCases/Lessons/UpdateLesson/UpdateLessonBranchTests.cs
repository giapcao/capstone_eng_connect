using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLesson;

public class UpdateLessonBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonTestData.HandlerBranchCases), MemberType = typeof(UpdateLessonTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateLessonTestData.ValidatorBranchCases), MemberType = typeof(UpdateLessonTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonTestData.Definition, caseSet);
    }
}