using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById;

public class GetPermissionRoleByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetPermissionRoleByIdTestData.Definition.ValidatorTypeFullName));
    }
}