using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.GetListCategory;

public class GetListCategoryValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListCategoryTestData.Definition.ValidatorTypeFullName));
    }
}