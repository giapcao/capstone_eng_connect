using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.CreatePermission;

public class CreatePermissionValidatorTests
{
    [Theory]
    [MemberData(nameof(CreatePermissionTestData.NormalValidatorCases), MemberType = typeof(CreatePermissionTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreatePermissionTestData.Definition, caseSet);
    }
}