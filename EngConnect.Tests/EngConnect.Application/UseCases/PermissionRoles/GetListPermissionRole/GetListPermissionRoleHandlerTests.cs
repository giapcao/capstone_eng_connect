using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole;

public class GetListPermissionRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListPermissionRoleTestData.NormalHandlerCases), MemberType = typeof(GetListPermissionRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListPermissionRoleTestData.Definition, caseSet);
    }
}