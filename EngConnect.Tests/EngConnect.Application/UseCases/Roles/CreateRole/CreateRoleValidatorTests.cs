using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.CreateRole;

public class CreateRoleValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateRoleTestData.NormalValidatorCases), MemberType = typeof(CreateRoleTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateRoleTestData.Definition, caseSet);
    }
}