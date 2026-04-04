using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.DeleteCategory;

public class DeleteCategoryValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteCategoryTestData.Definition.ValidatorTypeFullName));
    }
}