using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetListStudents;

internal enum GetListStudentsCase
{
    ValidDefault,
    ValidEmptyStatus,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class GetListStudentsTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListStudents",
        RequestTypeFullName = "EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQueryHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQueryValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Students/GetListStudents/GetListStudentQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Students/GetListStudents/GetListStudentQueryHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Students/GetListStudents/GetListStudentQueryValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListStudentsCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListStudentsCase.ValidEmptyStatus),
        BuildCase(GetListStudentsCase.BoundaryDefault),
        BuildCase(GetListStudentsCase.InvalidRequestShape),
        BuildCase(GetListStudentsCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListStudentsCase.ValidDefault),
        BuildCase(GetListStudentsCase.ValidEmptyStatus),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListStudentsCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetListStudentsCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListStudentsCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListStudentsCase caseId)
    {
        return caseId switch
        {
            GetListStudentsCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListStudentsCase.ValidEmptyStatus => CreateCase("valid-empty-Status", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListStudentsCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            GetListStudentsCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            GetListStudentsCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQuery CreateRequest(GetListStudentsCase caseId)
    {
        return caseId switch
        {
            GetListStudentsCase.ValidDefault => new global::EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQuery { Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListStudentsCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQuery { Status = "", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListStudentsCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQuery { Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListStudentsCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQuery { Status = "Invalid", PageNumber = 1, PageSize = 1, SearchTerm = "", SortParams = "" },
            GetListStudentsCase.ExceptionPath => new global::EngConnect.Application.UseCases.Students.GetListStudents.GetListStudentQuery { Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}