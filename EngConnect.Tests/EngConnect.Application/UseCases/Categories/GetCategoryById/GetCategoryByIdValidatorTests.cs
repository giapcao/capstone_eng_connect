using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.GetCategoryById;

public class GetCategoryByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetCategoryByIdTestData.Definition.ValidatorTypeFullName));
    }
}