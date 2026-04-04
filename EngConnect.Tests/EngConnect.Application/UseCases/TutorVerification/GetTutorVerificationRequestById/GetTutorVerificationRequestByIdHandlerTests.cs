using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.GetTutorVerificationRequestById;

public class GetTutorVerificationRequestByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetTutorVerificationRequestByIdTestData.NormalHandlerCases), MemberType = typeof(GetTutorVerificationRequestByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorVerificationRequestByIdTestData.Definition, caseSet);
    }
}