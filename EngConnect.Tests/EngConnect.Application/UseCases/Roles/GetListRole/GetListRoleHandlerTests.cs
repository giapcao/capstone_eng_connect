using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.GetListRole;

public class GetListRoleHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListRoleTestData.NormalHandlerCases), MemberType = typeof(GetListRoleTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListRoleTestData.Definition, caseSet);
    }
}