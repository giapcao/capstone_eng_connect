using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.GetListPermission;

public class GetListPermissionHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListPermissionTestData.NormalHandlerCases), MemberType = typeof(GetListPermissionTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListPermissionTestData.Definition, caseSet);
    }
}