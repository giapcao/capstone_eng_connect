using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetStudentById;

public class GetStudentByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetStudentByIdTestData.HandlerBranchCases), MemberType = typeof(GetStudentByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetStudentByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetStudentByIdTestData.Definition.ValidatorTypeFullName));
    }
}