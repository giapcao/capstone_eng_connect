using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetLessonById;

public class GetLessonByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetLessonByIdTestData.HandlerBranchCases), MemberType = typeof(GetLessonByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetLessonByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonByIdTestData.Definition.ValidatorTypeFullName));
    }
}