using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.CreatePermission;

public class CreatePermissionBranchTests
{
    [Theory]
    [MemberData(nameof(CreatePermissionTestData.HandlerBranchCases), MemberType = typeof(CreatePermissionTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreatePermissionTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreatePermissionTestData.ValidatorBranchCases), MemberType = typeof(CreatePermissionTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreatePermissionTestData.Definition, caseSet);
    }
}