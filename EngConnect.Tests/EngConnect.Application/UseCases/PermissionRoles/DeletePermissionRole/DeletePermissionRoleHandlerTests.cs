using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole;

public class DeletePermissionRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(DeletePermissionRoleTestData.NormalHandlerCases), MemberType = typeof(DeletePermissionRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeletePermissionRoleTestData.Definition, caseSet);
    }
}