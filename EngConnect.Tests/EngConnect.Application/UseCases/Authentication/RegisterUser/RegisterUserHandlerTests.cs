using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUser;

public class RegisterUserHandlerTests
{
    [Theory]
    [MemberData(nameof(RegisterUserTestData.NormalHandlerCases), MemberType = typeof(RegisterUserTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterUserTestData.Definition, caseSet);
    }
}