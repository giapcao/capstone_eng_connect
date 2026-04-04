using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.GetTutorDocumentById;

public class GetTutorDocumentByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorDocumentByIdTestData.Definition.ValidatorTypeFullName));
    }
}