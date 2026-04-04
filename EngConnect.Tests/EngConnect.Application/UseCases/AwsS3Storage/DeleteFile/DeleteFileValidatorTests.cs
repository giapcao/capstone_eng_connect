using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;

public class DeleteFileValidatorTests
{
    [Theory]
    [MemberData(nameof(DeleteFileTestData.NormalValidatorCases), MemberType = typeof(DeleteFileTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteFileTestData.Definition, caseSet);
    }
}