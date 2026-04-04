using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.UpdatePermission;

public class UpdatePermissionHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdatePermissionTestData.NormalHandlerCases), MemberType = typeof(UpdatePermissionTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdatePermissionTestData.Definition, caseSet);
    }
}