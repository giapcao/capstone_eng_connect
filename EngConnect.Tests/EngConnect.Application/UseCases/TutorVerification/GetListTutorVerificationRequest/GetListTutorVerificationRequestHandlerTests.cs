using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest;

public class GetListTutorVerificationRequestHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListTutorVerificationRequestTestData.NormalHandlerCases), MemberType = typeof(GetListTutorVerificationRequestTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorVerificationRequestTestData.Definition, caseSet);
    }
}