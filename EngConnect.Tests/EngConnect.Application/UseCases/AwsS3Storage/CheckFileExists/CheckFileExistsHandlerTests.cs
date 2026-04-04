using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;

public class CheckFileExistsHandlerTests
{
    [Theory]
    [MemberData(nameof(CheckFileExistsTestData.NormalHandlerCases), MemberType = typeof(CheckFileExistsTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CheckFileExistsTestData.Definition, caseSet);
    }
}