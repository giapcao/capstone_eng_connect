using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.CreateUserRole;

public class CreateUserRoleValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateUserRoleTestData.NormalValidatorCases), MemberType = typeof(CreateUserRoleTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateUserRoleTestData.Definition, caseSet);
    }
}