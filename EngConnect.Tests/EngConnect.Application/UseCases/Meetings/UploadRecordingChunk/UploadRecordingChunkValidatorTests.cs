using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

public class UploadRecordingChunkValidatorTests
{
    [Theory]
    [MemberData(nameof(UploadRecordingChunkTestData.NormalValidatorCases), MemberType = typeof(UploadRecordingChunkTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadRecordingChunkTestData.Definition, caseSet);
    }
}