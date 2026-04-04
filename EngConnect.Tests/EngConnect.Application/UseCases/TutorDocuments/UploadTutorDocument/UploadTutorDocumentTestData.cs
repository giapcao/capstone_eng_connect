using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

internal enum UploadTutorDocumentCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTutorExistsMissing,
    ExceptionPath,
}

internal static class UploadTutorDocumentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UploadTutorDocument",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/UploadTutorDocument/UploadTutorDocumentCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/UploadTutorDocument/UploadTutorDocumentCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/UploadTutorDocument/UploadTutorDocumentCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UploadTutorDocumentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UploadTutorDocumentCase.BoundaryDefault),
        BuildCase(UploadTutorDocumentCase.InvalidRequestShape),
        BuildCase(UploadTutorDocumentCase.InvalidTutorExistsMissing),
        BuildCase(UploadTutorDocumentCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UploadTutorDocumentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UploadTutorDocumentCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UploadTutorDocumentCase.InvalidRequestShape),
        BuildCase(UploadTutorDocumentCase.InvalidTutorExistsMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UploadTutorDocumentCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UploadTutorDocumentCase caseId)
    {
        return caseId switch
        {
            UploadTutorDocumentCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadTutorDocumentCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadTutorDocumentCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UploadTutorDocumentCase.InvalidTutorExistsMissing => CreateCase("invalid-tutorExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UploadTutorDocumentCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommand CreateRequest(UploadTutorDocumentCase caseId)
    {
        return caseId switch
        {
            UploadTutorDocumentCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommand { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Sample", DocType = "General", IssuedBy = "SampleValue", IssuedAt = new DateOnly(2026, 1, 1), ExpiredAt = new DateOnly(2026, 1, 1), Status = "Active", File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadTutorDocumentCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommand { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Sample", DocType = "General", IssuedBy = "SampleValue", IssuedAt = new DateOnly(2026, 1, 1), ExpiredAt = new DateOnly(2026, 1, 1), Status = "Active", File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadTutorDocumentCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommand { TutorId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Name = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", DocType = "Invalid", IssuedBy = "", IssuedAt = new DateOnly(2026, 1, 1), ExpiredAt = new DateOnly(2026, 1, 1), Status = "Invalid", File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume", ContentType = "application/pdf", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadTutorDocumentCase.InvalidTutorExistsMissing => new global::EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommand { TutorId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Sample", DocType = "General", IssuedBy = "SampleValue", IssuedAt = new DateOnly(2026, 1, 1), ExpiredAt = new DateOnly(2026, 1, 1), Status = "Active", File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            UploadTutorDocumentCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument.UploadTutorDocumentCommand { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Sample", DocType = "General", IssuedBy = "SampleValue", IssuedAt = new DateOnly(2026, 1, 1), ExpiredAt = new DateOnly(2026, 1, 1), Status = "Active", File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "resume.pdf", ContentType = "application/pdf", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}