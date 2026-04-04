using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetListUser;

public class GetListUserValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListUserTestData.Definition.ValidatorTypeFullName));
    }
}