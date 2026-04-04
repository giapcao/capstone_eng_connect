using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

internal enum UploadRecordingChunkCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidLessonMissing,
    ExceptionPath,
}

internal static class UploadRecordingChunkTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UploadRecordingChunk",
        RequestTypeFullName = "EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Meetings/UploadRecordingChunk/UploadRecordingChunkCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Meetings/UploadRecordingChunk/UploadRecordingChunkCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Meetings/UploadRecordingChunk/UploadRecordingChunkCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UploadRecordingChunkCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UploadRecordingChunkCase.BoundaryDefault),
        BuildCase(UploadRecordingChunkCase.InvalidRequestShape),
        BuildCase(UploadRecordingChunkCase.InvalidLessonMissing),
        BuildCase(UploadRecordingChunkCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UploadRecordingChunkCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UploadRecordingChunkCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UploadRecordingChunkCase.InvalidRequestShape),
        BuildCase(UploadRecordingChunkCase.InvalidLessonMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UploadRecordingChunkCase.ExceptionPath),
    ];

    public static IEnumerable<object[]> NormalHandlerCases()
    {
        return NormalCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> NormalValidatorCases()
    {
        return NormalCases
            .Where(caseSet => caseSet.ValidatorExpectation != UseCaseValidatorExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> HandlerBranchCases()
    {
        return BranchCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> ValidatorBranchCases()
    {
        return BranchCases
            .Where(caseSet => caseSet.ValidatorExpectation != UseCaseValidatorExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    private static UseCaseCaseSet BuildCase(UploadRecordingChunkCase caseId)
    {
        return caseId switch
        {
            UploadRecordingChunkCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadRecordingChunkCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadRecordingChunkCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UploadRecordingChunkCase.InvalidLessonMissing => CreateCase("invalid-lesson-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadRecordingChunkCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateCase(
        string name,
        UseCaseCaseKind kind,
        UseCaseHandlerExpectation handlerExpectation,
        UseCaseValidatorExpectation validatorExpectation,
        object request)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = handlerExpectation,
            ValidatorExpectation = validatorExpectation,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request
                }
            }
        };
    }

    private static global::EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommand CreateRequest(UploadRecordingChunkCase caseId)
    {
        return caseId switch
        {
            UploadRecordingChunkCase.ValidDefault => new global::EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ChunkTimestamp = 1, File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "recording.webm", ContentType = "audio/webm", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadRecordingChunkCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ChunkTimestamp = 1, File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "recording.webm", ContentType = "audio/webm", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadRecordingChunkCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommand { LessonId = Guid.Parse("00000000-0000-0000-0000-000000000000"), UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), ChunkTimestamp = 0, File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "recording", ContentType = "audio/webm", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadRecordingChunkCase.InvalidLessonMissing => new global::EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommand { LessonId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ChunkTimestamp = 1, File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "recording.webm", ContentType = "audio/webm", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadRecordingChunkCase.ExceptionPath => new global::EngConnect.Application.UseCases.Meetings.UploadRecordingChunk.UploadRecordingChunkCommand { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), ChunkTimestamp = 1, File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "recording.webm", ContentType = "audio/webm", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}