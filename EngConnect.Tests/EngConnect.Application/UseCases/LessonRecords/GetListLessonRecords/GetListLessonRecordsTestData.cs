using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords;

internal enum GetListLessonRecordsCase
{
    ValidDefault,
    ValidWithoutLessonId,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class GetListLessonRecordsTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListLessonRecords",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQueryHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQueryValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/GetListLessonRecords/GetListLessonRecordQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/GetListLessonRecords/GetListLessonRecordQueryHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/LessonRecords/GetListLessonRecords/GetListLessonRecordQueryValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListLessonRecordsCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListLessonRecordsCase.ValidWithoutLessonId),
        BuildCase(GetListLessonRecordsCase.BoundaryDefault),
        BuildCase(GetListLessonRecordsCase.InvalidRequestShape),
        BuildCase(GetListLessonRecordsCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListLessonRecordsCase.ValidDefault),
        BuildCase(GetListLessonRecordsCase.ValidWithoutLessonId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListLessonRecordsCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetListLessonRecordsCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListLessonRecordsCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListLessonRecordsCase caseId)
    {
        return caseId switch
        {
            GetListLessonRecordsCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonRecordsCase.ValidWithoutLessonId => CreateCase("valid-without-LessonId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonRecordsCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonRecordsCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetListLessonRecordsCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQuery CreateRequest(GetListLessonRecordsCase caseId)
    {
        return caseId switch
        {
            GetListLessonRecordsCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRecordsCase.ValidWithoutLessonId => new global::EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQuery { LessonId = null, PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRecordsCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonRecordsCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQuery { LessonId = Guid.Parse("00000000-0000-0000-0000-000000000000"), PageNumber = 1, PageSize = 1, SearchTerm = "", SortParams = "" },
            GetListLessonRecordsCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords.GetListLessonRecordQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}