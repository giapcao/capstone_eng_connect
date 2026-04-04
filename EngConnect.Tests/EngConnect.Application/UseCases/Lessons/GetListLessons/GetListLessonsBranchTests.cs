using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetListLessons;

public class GetListLessonsBranchTests
{
    [Theory]
    [MemberData(nameof(GetListLessonsTestData.HandlerBranchCases), MemberType = typeof(GetListLessonsTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListLessonsTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetListLessonsTestData.ValidatorBranchCases), MemberType = typeof(GetListLessonsTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetListLessonsTestData.Definition, caseSet);
    }
}