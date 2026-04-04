using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest;

public class CreateTutorVerificationRequestValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateTutorVerificationRequestTestData.NormalValidatorCases), MemberType = typeof(CreateTutorVerificationRequestTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateTutorVerificationRequestTestData.Definition, caseSet);
    }
}