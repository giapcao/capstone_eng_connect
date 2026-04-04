using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.UpdateRole;

public class UpdateRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateRoleTestData.NormalHandlerCases), MemberType = typeof(UpdateRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateRoleTestData.Definition, caseSet);
    }
}