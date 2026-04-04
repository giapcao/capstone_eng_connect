using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.GetListUserRole;

public class GetListUserRoleValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListUserRoleTestData.Definition.ValidatorTypeFullName));
    }
}