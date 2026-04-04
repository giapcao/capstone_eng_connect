using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.GetPermissionById;

public class GetPermissionByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetPermissionByIdTestData.NormalHandlerCases), MemberType = typeof(GetPermissionByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetPermissionByIdTestData.Definition, caseSet);
    }
}