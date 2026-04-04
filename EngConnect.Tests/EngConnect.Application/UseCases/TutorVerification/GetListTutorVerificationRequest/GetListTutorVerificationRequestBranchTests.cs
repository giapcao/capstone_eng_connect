using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest;

public class GetListTutorVerificationRequestBranchTests
{
    [Theory]
    [MemberData(nameof(GetListTutorVerificationRequestTestData.HandlerBranchCases), MemberType = typeof(GetListTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorVerificationRequestTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorVerificationRequestTestData.Definition.ValidatorTypeFullName));
    }
}