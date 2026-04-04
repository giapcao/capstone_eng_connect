using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.CreatePermission;

public class CreatePermissionHandlerTests
{
    [Theory]
    [MemberData(nameof(CreatePermissionTestData.NormalHandlerCases), MemberType = typeof(CreatePermissionTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreatePermissionTestData.Definition, caseSet);
    }
}