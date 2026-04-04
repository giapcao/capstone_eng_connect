using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole;

public class CreatePermissionRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(CreatePermissionRoleTestData.NormalHandlerCases), MemberType = typeof(CreatePermissionRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreatePermissionRoleTestData.Definition, caseSet);
    }
}