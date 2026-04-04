using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole;

public class CreatePermissionRoleValidatorTests
{
    [Theory]
    [MemberData(nameof(CreatePermissionRoleTestData.NormalValidatorCases), MemberType = typeof(CreatePermissionRoleTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreatePermissionRoleTestData.Definition, caseSet);
    }
}