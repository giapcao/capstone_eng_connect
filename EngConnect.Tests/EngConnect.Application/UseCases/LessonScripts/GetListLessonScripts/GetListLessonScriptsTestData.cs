using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;

internal enum GetListLessonScriptsCase
{
    ValidDefault,
    ValidWithoutLessonId,
    ValidWithoutRecordId,
    ValidEmptyLanguage,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class GetListLessonScriptsTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListLessonScripts",
        RequestTypeFullName = "EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQueryHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQueryValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/GetListLessonScripts/GetListLessonScriptsQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/GetListLessonScripts/GetListLessonScriptsQueryHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/LessonScripts/GetListLessonScripts/GetListLessonScriptsQueryValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListLessonScriptsCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListLessonScriptsCase.ValidWithoutLessonId),
        BuildCase(GetListLessonScriptsCase.ValidWithoutRecordId),
        BuildCase(GetListLessonScriptsCase.ValidEmptyLanguage),
        BuildCase(GetListLessonScriptsCase.BoundaryDefault),
        BuildCase(GetListLessonScriptsCase.InvalidRequestShape),
        BuildCase(GetListLessonScriptsCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListLessonScriptsCase.ValidDefault),
        BuildCase(GetListLessonScriptsCase.ValidWithoutLessonId),
        BuildCase(GetListLessonScriptsCase.ValidWithoutRecordId),
        BuildCase(GetListLessonScriptsCase.ValidEmptyLanguage),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListLessonScriptsCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetListLessonScriptsCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListLessonScriptsCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListLessonScriptsCase caseId)
    {
        return caseId switch
        {
            GetListLessonScriptsCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonScriptsCase.ValidWithoutLessonId => CreateCase("valid-without-LessonId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonScriptsCase.ValidWithoutRecordId => CreateCase("valid-without-RecordId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonScriptsCase.ValidEmptyLanguage => CreateCase("valid-empty-Language", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonScriptsCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListLessonScriptsCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetListLessonScriptsCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery CreateRequest(GetListLessonScriptsCase caseId)
    {
        return caseId switch
        {
            GetListLessonScriptsCase.ValidDefault => new global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonScriptsCase.ValidWithoutLessonId => new global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery { LessonId = null, RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonScriptsCase.ValidWithoutRecordId => new global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = null, Language = "En", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonScriptsCase.ValidEmptyLanguage => new global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonScriptsCase.BoundaryDefault => new global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListLessonScriptsCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery { LessonId = Guid.Parse("00000000-0000-0000-0000-000000000000"), RecordId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Language = "Invalid", PageNumber = 1, PageSize = 1, SearchTerm = "", SortParams = "" },
            GetListLessonScriptsCase.ExceptionPath => new global::EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts.GetListLessonScriptsQuery { LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RecordId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Language = "En", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}