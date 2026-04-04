using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;

public class DeleteFileFromDriveHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteFileFromDriveTestData.NormalHandlerCases), MemberType = typeof(DeleteFileFromDriveTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteFileFromDriveTestData.Definition, caseSet);
    }
}