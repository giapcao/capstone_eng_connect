using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ChangePassword;

public class ChangePasswordValidatorTests
{
    [Theory]
    [MemberData(nameof(ChangePasswordTestData.NormalValidatorCases), MemberType = typeof(ChangePasswordTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ChangePasswordTestData.Definition, caseSet);
    }
}