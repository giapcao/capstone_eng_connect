using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.CreateRole;

public class CreateRoleBranchTests
{
    [Theory]
    [MemberData(nameof(CreateRoleTestData.HandlerBranchCases), MemberType = typeof(CreateRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateRoleTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateRoleTestData.ValidatorBranchCases), MemberType = typeof(CreateRoleTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateRoleTestData.Definition, caseSet);
    }
}