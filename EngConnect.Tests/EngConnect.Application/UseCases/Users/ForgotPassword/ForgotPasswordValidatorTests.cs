using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ForgotPassword;

public class ForgotPasswordValidatorTests
{
    [Theory]
    [MemberData(nameof(ForgotPasswordTestData.NormalValidatorCases), MemberType = typeof(ForgotPasswordTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ForgotPasswordTestData.Definition, caseSet);
    }
}