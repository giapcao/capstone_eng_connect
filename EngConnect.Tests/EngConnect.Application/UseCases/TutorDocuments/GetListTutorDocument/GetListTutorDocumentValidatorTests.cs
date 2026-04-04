using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument;

public class GetListTutorDocumentValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorDocumentTestData.Definition.ValidatorTypeFullName));
    }
}