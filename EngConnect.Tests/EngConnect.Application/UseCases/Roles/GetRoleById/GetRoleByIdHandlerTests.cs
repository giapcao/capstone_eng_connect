using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.GetRoleById;

public class GetRoleByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetRoleByIdTestData.NormalHandlerCases), MemberType = typeof(GetRoleByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetRoleByIdTestData.Definition, caseSet);
    }
}