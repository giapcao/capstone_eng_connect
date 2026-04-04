using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.GetListUserRole;

public class GetListUserRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListUserRoleTestData.NormalHandlerCases), MemberType = typeof(GetListUserRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListUserRoleTestData.Definition, caseSet);
    }
}