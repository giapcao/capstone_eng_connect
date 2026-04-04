using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;

public class UploadFileToDriveHandlerTests
{
    [Theory]
    [MemberData(nameof(UploadFileToDriveTestData.NormalHandlerCases), MemberType = typeof(UploadFileToDriveTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadFileToDriveTestData.Definition, caseSet);
    }
}