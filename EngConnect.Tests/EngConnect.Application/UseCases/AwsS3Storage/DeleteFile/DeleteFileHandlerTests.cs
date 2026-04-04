using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;

public class DeleteFileHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteFileTestData.NormalHandlerCases), MemberType = typeof(DeleteFileTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteFileTestData.Definition, caseSet);
    }
}