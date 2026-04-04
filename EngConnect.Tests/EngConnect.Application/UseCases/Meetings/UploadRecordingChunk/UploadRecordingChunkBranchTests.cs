using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

public class UploadRecordingChunkBranchTests
{
    [Theory]
    [MemberData(nameof(UploadRecordingChunkTestData.HandlerBranchCases), MemberType = typeof(UploadRecordingChunkTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadRecordingChunkTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UploadRecordingChunkTestData.ValidatorBranchCases), MemberType = typeof(UploadRecordingChunkTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadRecordingChunkTestData.Definition, caseSet);
    }
}