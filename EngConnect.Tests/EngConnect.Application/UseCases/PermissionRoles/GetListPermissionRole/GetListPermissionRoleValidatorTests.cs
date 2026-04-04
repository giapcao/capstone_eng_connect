using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole;

public class GetListPermissionRoleValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListPermissionRoleTestData.Definition.ValidatorTypeFullName));
    }
}