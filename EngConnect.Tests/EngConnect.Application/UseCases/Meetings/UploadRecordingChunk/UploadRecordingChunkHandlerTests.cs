using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

public class UploadRecordingChunkHandlerTests
{
    [Theory]
    [MemberData(nameof(UploadRecordingChunkTestData.NormalHandlerCases), MemberType = typeof(UploadRecordingChunkTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadRecordingChunkTestData.Definition, caseSet);
    }
}