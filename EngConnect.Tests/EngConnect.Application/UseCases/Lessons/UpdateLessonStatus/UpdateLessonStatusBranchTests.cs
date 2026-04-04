using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;

public class UpdateLessonStatusBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateLessonStatusTestData.HandlerBranchCases), MemberType = typeof(UpdateLessonStatusTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateLessonStatusTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateLessonStatusTestData.ValidatorBranchCases), MemberType = typeof(UpdateLessonStatusTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateLessonStatusTestData.Definition, caseSet);
    }
}