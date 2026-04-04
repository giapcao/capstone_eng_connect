using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.UpdateRole;

public class UpdateRoleBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateRoleTestData.HandlerBranchCases), MemberType = typeof(UpdateRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateRoleTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateRoleTestData.ValidatorBranchCases), MemberType = typeof(UpdateRoleTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateRoleTestData.Definition, caseSet);
    }
}