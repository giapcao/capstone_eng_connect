using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.DeletePermission;

public class DeletePermissionHandlerTests
{
    [Theory]
    [MemberData(nameof(DeletePermissionTestData.NormalHandlerCases), MemberType = typeof(DeletePermissionTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeletePermissionTestData.Definition, caseSet);
    }
}