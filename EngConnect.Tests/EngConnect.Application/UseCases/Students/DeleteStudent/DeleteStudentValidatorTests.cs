using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.DeleteStudent;

public class DeleteStudentValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteStudentTestData.Definition.ValidatorTypeFullName));
    }
}