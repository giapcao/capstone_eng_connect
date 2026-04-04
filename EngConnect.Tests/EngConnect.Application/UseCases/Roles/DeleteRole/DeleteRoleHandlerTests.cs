using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.DeleteRole;

public class DeleteRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteRoleTestData.NormalHandlerCases), MemberType = typeof(DeleteRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteRoleTestData.Definition, caseSet);
    }
}