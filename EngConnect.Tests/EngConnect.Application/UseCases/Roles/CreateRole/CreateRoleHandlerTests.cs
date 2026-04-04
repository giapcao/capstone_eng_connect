using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.CreateRole;

public class CreateRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateRoleTestData.NormalHandlerCases), MemberType = typeof(CreateRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateRoleTestData.Definition, caseSet);
    }
}