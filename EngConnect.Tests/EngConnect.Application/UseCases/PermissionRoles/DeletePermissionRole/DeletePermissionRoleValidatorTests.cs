using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole;

public class DeletePermissionRoleValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeletePermissionRoleTestData.Definition.ValidatorTypeFullName));
    }
}