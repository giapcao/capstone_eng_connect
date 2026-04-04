using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest;

public class CreateTutorVerificationRequestBranchTests
{
    [Theory]
    [MemberData(nameof(CreateTutorVerificationRequestTestData.HandlerBranchCases), MemberType = typeof(CreateTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateTutorVerificationRequestTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateTutorVerificationRequestTestData.ValidatorBranchCases), MemberType = typeof(CreateTutorVerificationRequestTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateTutorVerificationRequestTestData.Definition, caseSet);
    }
}