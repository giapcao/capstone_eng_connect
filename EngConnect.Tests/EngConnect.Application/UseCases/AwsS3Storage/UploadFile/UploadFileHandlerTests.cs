using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.UploadFile;

public class UploadFileHandlerTests
{
    [Theory]
    [MemberData(nameof(UploadFileTestData.NormalHandlerCases), MemberType = typeof(UploadFileTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadFileTestData.Definition, caseSet);
    }
}