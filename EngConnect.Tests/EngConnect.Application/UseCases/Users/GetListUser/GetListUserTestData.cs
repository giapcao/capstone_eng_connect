using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetListUser;

internal enum GetListUserCase
{
    ValidDefault,
    ValidEmptyStatus,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListUserTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListUser",
        RequestTypeFullName = "EngConnect.Application.UseCases.Users.GetListUser.GetListUserQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Users.GetListUser.GetListUserQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Users/GetListUser/GetListUserQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Users/GetListUser/GetListUserQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListUserCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListUserCase.ValidEmptyStatus),
        BuildCase(GetListUserCase.BoundaryDefault),
        BuildCase(GetListUserCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListUserCase.ValidDefault),
        BuildCase(GetListUserCase.ValidEmptyStatus),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListUserCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListUserCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListUserCase caseId)
    {
        return caseId switch
        {
            GetListUserCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListUserCase.ValidEmptyStatus => CreateCase("valid-empty-Status", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListUserCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListUserCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Users.GetListUser.GetListUserQuery CreateRequest(GetListUserCase caseId)
    {
        return caseId switch
        {
            GetListUserCase.ValidDefault => new global::EngConnect.Application.UseCases.Users.GetListUser.GetListUserQuery { Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListUserCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.Users.GetListUser.GetListUserQuery { Status = "", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListUserCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Users.GetListUser.GetListUserQuery { Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListUserCase.ExceptionPath => new global::EngConnect.Application.UseCases.Users.GetListUser.GetListUserQuery { Status = "Active", PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}