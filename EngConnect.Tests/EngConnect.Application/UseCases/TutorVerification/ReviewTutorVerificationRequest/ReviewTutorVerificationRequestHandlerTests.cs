using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest;

public class ReviewTutorVerificationRequestHandlerTests
{
    [Theory]
    [MemberData(nameof(ReviewTutorVerificationRequestTestData.NormalHandlerCases), MemberType = typeof(ReviewTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ReviewTutorVerificationRequestTestData.Definition, caseSet);
    }
}