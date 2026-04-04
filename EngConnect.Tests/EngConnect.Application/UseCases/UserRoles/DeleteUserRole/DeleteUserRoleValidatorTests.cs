using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.DeleteUserRole;

public class DeleteUserRoleValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteUserRoleTestData.Definition.ValidatorTypeFullName));
    }
}