using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument;

internal enum GetListTutorDocumentCase
{
    ValidDefault,
    ValidWithoutTutorId,
    ValidEmptyDocType,
    ValidEmptyStatus,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListTutorDocumentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListTutorDocument",
        RequestTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/GetListTutorDocument/GetListTutorDocumentQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/TutorDocuments/GetListTutorDocument/GetListTutorDocumentQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListTutorDocumentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListTutorDocumentCase.ValidWithoutTutorId),
        BuildCase(GetListTutorDocumentCase.ValidEmptyDocType),
        BuildCase(GetListTutorDocumentCase.ValidEmptyStatus),
        BuildCase(GetListTutorDocumentCase.BoundaryDefault),
        BuildCase(GetListTutorDocumentCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListTutorDocumentCase.ValidDefault),
        BuildCase(GetListTutorDocumentCase.ValidWithoutTutorId),
        BuildCase(GetListTutorDocumentCase.ValidEmptyDocType),
        BuildCase(GetListTutorDocumentCase.ValidEmptyStatus),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListTutorDocumentCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListTutorDocumentCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListTutorDocumentCase caseId)
    {
        return caseId switch
        {
            GetListTutorDocumentCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorDocumentCase.ValidWithoutTutorId => CreateCase("valid-without-TutorId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorDocumentCase.ValidEmptyDocType => CreateCase("valid-empty-DocType", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorDocumentCase.ValidEmptyStatus => CreateCase("valid-empty-Status", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorDocumentCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListTutorDocumentCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery CreateRequest(GetListTutorDocumentCase caseId)
    {
        return caseId switch
        {
            GetListTutorDocumentCase.ValidDefault => new global::EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), DocType = "General", Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorDocumentCase.ValidWithoutTutorId => new global::EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery { TutorId = null, DocType = "General", Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorDocumentCase.ValidEmptyDocType => new global::EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), DocType = "", Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorDocumentCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), DocType = "General", Status = "", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorDocumentCase.BoundaryDefault => new global::EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), DocType = "General", Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListTutorDocumentCase.ExceptionPath => new global::EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument.GetListTutorDocumentQuery { TutorId = Guid.Parse("11111111-1111-1111-1111-111111111111"), DocType = "General", Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}