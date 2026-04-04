using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest;

public class ReviewTutorVerificationRequestValidatorTests
{
    [Theory]
    [MemberData(nameof(ReviewTutorVerificationRequestTestData.NormalValidatorCases), MemberType = typeof(ReviewTutorVerificationRequestTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ReviewTutorVerificationRequestTestData.Definition, caseSet);
    }
}