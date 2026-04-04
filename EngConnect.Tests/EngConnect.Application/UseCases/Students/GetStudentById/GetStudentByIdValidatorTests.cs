using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetStudentById;

public class GetStudentByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetStudentByIdTestData.Definition.ValidatorTypeFullName));
    }
}