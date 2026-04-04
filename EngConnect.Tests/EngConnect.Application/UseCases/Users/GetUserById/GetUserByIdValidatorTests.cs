using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetUserById;

public class GetUserByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetUserByIdTestData.Definition.ValidatorTypeFullName));
    }
}