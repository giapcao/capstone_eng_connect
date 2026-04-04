using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.UpdatePermission;

public class UpdatePermissionValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdatePermissionTestData.NormalValidatorCases), MemberType = typeof(UpdatePermissionTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdatePermissionTestData.Definition, caseSet);
    }
}