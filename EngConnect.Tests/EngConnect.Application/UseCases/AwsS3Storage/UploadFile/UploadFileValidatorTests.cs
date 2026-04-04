using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.UploadFile;

public class UploadFileValidatorTests
{
    [Theory]
    [MemberData(nameof(UploadFileTestData.NormalValidatorCases), MemberType = typeof(UploadFileTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadFileTestData.Definition, caseSet);
    }
}