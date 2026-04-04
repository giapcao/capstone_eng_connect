using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest;

public class ReviewTutorVerificationRequestBranchTests
{
    [Theory]
    [MemberData(nameof(ReviewTutorVerificationRequestTestData.HandlerBranchCases), MemberType = typeof(ReviewTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ReviewTutorVerificationRequestTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(ReviewTutorVerificationRequestTestData.ValidatorBranchCases), MemberType = typeof(ReviewTutorVerificationRequestTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ReviewTutorVerificationRequestTestData.Definition, caseSet);
    }
}