using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest;

public class DeleteTutorVerificationRequestBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteTutorVerificationRequestTestData.HandlerBranchCases), MemberType = typeof(DeleteTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteTutorVerificationRequestTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteTutorVerificationRequestTestData.Definition.ValidatorTypeFullName));
    }
}