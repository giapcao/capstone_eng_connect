using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.GetTutorVerificationRequestById;

public class GetTutorVerificationRequestByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetTutorVerificationRequestByIdTestData.HandlerBranchCases), MemberType = typeof(GetTutorVerificationRequestByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorVerificationRequestByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorVerificationRequestByIdTestData.Definition.ValidatorTypeFullName));
    }
}