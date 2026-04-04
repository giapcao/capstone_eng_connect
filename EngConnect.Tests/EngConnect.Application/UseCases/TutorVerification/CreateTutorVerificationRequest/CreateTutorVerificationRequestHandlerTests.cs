using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest;

public class CreateTutorVerificationRequestHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateTutorVerificationRequestTestData.NormalHandlerCases), MemberType = typeof(CreateTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateTutorVerificationRequestTestData.Definition, caseSet);
    }
}