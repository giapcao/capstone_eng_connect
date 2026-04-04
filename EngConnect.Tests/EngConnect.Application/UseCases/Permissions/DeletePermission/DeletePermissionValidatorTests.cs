using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.DeletePermission;

public class DeletePermissionValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeletePermissionTestData.Definition.ValidatorTypeFullName));
    }
}