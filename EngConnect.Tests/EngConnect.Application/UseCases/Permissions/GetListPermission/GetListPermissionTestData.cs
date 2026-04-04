using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.GetListPermission;

internal enum GetListPermissionCase
{
    ValidDefault,
    ValidEmptyCode,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListPermissionTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListPermission",
        RequestTypeFullName = "EngConnect.Application.UseCases.Permissions.GetListPermission.GetListPermissionQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Permissions.GetListPermission.GetListPermissionQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Permissions/GetListPermission/GetListPermissionQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Permissions/GetListPermission/GetListPermissionQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListPermissionCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListPermissionCase.ValidEmptyCode),
        BuildCase(GetListPermissionCase.BoundaryDefault),
        BuildCase(GetListPermissionCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListPermissionCase.ValidDefault),
        BuildCase(GetListPermissionCase.ValidEmptyCode),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListPermissionCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListPermissionCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListPermissionCase caseId)
    {
        return caseId switch
        {
            GetListPermissionCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListPermissionCase.ValidEmptyCode => CreateCase("valid-empty-Code", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListPermissionCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListPermissionCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Permissions.GetListPermission.GetListPermissionQuery CreateRequest(GetListPermissionCase caseId)
    {
        return caseId switch
        {
            GetListPermissionCase.ValidDefault => new global::EngConnect.Application.UseCases.Permissions.GetListPermission.GetListPermissionQuery { Code = "TEST-CODE", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListPermissionCase.ValidEmptyCode => new global::EngConnect.Application.UseCases.Permissions.GetListPermission.GetListPermissionQuery { Code = "", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListPermissionCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Permissions.GetListPermission.GetListPermissionQuery { Code = "TEST-CODE", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListPermissionCase.ExceptionPath => new global::EngConnect.Application.UseCases.Permissions.GetListPermission.GetListPermissionQuery { Code = "TEST-CODE", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}