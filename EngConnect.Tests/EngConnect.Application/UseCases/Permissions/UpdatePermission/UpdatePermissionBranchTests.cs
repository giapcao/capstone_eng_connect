using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.UpdatePermission;

public class UpdatePermissionBranchTests
{
    [Theory]
    [MemberData(nameof(UpdatePermissionTestData.HandlerBranchCases), MemberType = typeof(UpdatePermissionTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdatePermissionTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdatePermissionTestData.ValidatorBranchCases), MemberType = typeof(UpdatePermissionTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdatePermissionTestData.Definition, caseSet);
    }
}