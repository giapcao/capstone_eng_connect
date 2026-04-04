using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUserStaff;

public class RegisterUserStaffValidatorTests
{
    [Theory]
    [MemberData(nameof(RegisterUserStaffTestData.NormalValidatorCases), MemberType = typeof(RegisterUserStaffTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterUserStaffTestData.Definition, caseSet);
    }
}