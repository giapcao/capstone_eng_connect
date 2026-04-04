using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;

public class GetFileFromDriveHandlerTests
{
    [Theory]
    [MemberData(nameof(GetFileFromDriveTestData.NormalHandlerCases), MemberType = typeof(GetFileFromDriveTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetFileFromDriveTestData.Definition, caseSet);
    }
}