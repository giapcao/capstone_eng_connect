using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;

internal enum RemoveTutorDocumentCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidEntityMissing,
    ExceptionPath,
}

internal static class RemoveTutorDocumentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "RemoveTutorDocument",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/RemoveTutorDocument/RemoveTutorDocumentCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/RemoveTutorDocument/RemoveTutorDocumentCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/RemoveTutorDocument/RemoveTutorDocumentCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(RemoveTutorDocumentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(RemoveTutorDocumentCase.BoundaryDefault),
        BuildCase(RemoveTutorDocumentCase.InvalidRequestShape),
        BuildCase(RemoveTutorDocumentCase.InvalidEntityMissing),
        BuildCase(RemoveTutorDocumentCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(RemoveTutorDocumentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(RemoveTutorDocumentCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(RemoveTutorDocumentCase.InvalidRequestShape),
        BuildCase(RemoveTutorDocumentCase.InvalidEntityMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(RemoveTutorDocumentCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(RemoveTutorDocumentCase caseId)
    {
        return caseId switch
        {
            RemoveTutorDocumentCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RemoveTutorDocumentCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RemoveTutorDocumentCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            RemoveTutorDocumentCase.InvalidEntityMissing => CreateCase("invalid-entity-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RemoveTutorDocumentCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommand CreateRequest(RemoveTutorDocumentCase caseId)
    {
        return caseId switch
        {
            RemoveTutorDocumentCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommand { DocumentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            RemoveTutorDocumentCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommand { DocumentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            RemoveTutorDocumentCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommand { DocumentId = Guid.Parse("00000000-0000-0000-0000-000000000000"), TutorId = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            RemoveTutorDocumentCase.InvalidEntityMissing => new global::EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommand { DocumentId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), TutorId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            RemoveTutorDocumentCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument.RemoveTutorDocumentCommand { DocumentId = Guid.Parse("11111111-1111-1111-1111-111111111111"), TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}