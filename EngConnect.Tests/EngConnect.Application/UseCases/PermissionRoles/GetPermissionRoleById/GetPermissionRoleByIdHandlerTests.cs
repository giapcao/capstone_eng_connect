using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById;

public class GetPermissionRoleByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetPermissionRoleByIdTestData.NormalHandlerCases), MemberType = typeof(GetPermissionRoleByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetPermissionRoleByIdTestData.Definition, caseSet);
    }
}