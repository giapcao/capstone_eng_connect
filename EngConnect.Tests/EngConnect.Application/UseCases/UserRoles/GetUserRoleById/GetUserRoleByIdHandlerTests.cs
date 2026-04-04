using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.GetUserRoleById;

public class GetUserRoleByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetUserRoleByIdTestData.NormalHandlerCases), MemberType = typeof(GetUserRoleByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetUserRoleByIdTestData.Definition, caseSet);
    }
}