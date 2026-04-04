using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetTutorById;

public class GetTutorByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetTutorByIdTestData.HandlerBranchCases), MemberType = typeof(GetTutorByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorByIdTestData.Definition.ValidatorTypeFullName));
    }
}