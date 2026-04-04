using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.GetListPermission;

public class GetListPermissionValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListPermissionTestData.Definition.ValidatorTypeFullName));
    }
}