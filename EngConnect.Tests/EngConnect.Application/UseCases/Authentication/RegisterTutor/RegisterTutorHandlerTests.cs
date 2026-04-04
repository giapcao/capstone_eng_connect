using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterTutor;

public class RegisterTutorHandlerTests
{
    [Theory]
    [MemberData(nameof(RegisterTutorTestData.NormalHandlerCases), MemberType = typeof(RegisterTutorTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterTutorTestData.Definition, caseSet);
    }
}