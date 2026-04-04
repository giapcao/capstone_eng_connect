using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ResetPassword;

public class ResetPasswordValidatorTests
{
    [Theory]
    [MemberData(nameof(ResetPasswordTestData.NormalValidatorCases), MemberType = typeof(ResetPasswordTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ResetPasswordTestData.Definition, caseSet);
    }
}