using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateStatusStudent;

public class UpdateStatusStudentValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(UpdateStatusStudentTestData.Definition.ValidatorTypeFullName));
    }
}