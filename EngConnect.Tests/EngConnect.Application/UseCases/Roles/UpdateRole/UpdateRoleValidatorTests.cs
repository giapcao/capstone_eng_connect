using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.UpdateRole;

public class UpdateRoleValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateRoleTestData.NormalValidatorCases), MemberType = typeof(UpdateRoleTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateRoleTestData.Definition, caseSet);
    }
}