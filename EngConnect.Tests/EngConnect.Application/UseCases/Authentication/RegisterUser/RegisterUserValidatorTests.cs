using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUser;

public class RegisterUserValidatorTests
{
    [Theory]
    [MemberData(nameof(RegisterUserTestData.NormalValidatorCases), MemberType = typeof(RegisterUserTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterUserTestData.Definition, caseSet);
    }
}