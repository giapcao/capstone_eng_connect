using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.CreateUserRole;

public class CreateUserRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateUserRoleTestData.NormalHandlerCases), MemberType = typeof(CreateUserRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateUserRoleTestData.Definition, caseSet);
    }
}