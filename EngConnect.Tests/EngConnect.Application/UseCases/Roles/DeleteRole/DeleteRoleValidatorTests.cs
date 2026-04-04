using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.DeleteRole;

public class DeleteRoleValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteRoleTestData.Definition.ValidatorTypeFullName));
    }
}