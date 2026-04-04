using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.DeleteTutor;

public class DeleteTutorValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteTutorTestData.Definition.ValidatorTypeFullName));
    }
}