using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.DeleteUserRole;

public class DeleteUserRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteUserRoleTestData.NormalHandlerCases), MemberType = typeof(DeleteUserRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteUserRoleTestData.Definition, caseSet);
    }
}