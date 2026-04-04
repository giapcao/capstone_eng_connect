using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest;

public class DeleteTutorVerificationRequestHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteTutorVerificationRequestTestData.NormalHandlerCases), MemberType = typeof(DeleteTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteTutorVerificationRequestTestData.Definition, caseSet);
    }
}