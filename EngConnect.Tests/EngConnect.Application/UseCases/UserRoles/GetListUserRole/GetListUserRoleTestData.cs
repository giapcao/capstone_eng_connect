using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.GetListUserRole;

internal enum GetListUserRoleCase
{
    ValidDefault,
    ValidWithoutUserId,
    ValidWithoutRoleId,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListUserRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListUserRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/UserRoles/GetListUserRole/GetListUserRoleQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/UserRoles/GetListUserRole/GetListUserRoleQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListUserRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListUserRoleCase.ValidWithoutUserId),
        BuildCase(GetListUserRoleCase.ValidWithoutRoleId),
        BuildCase(GetListUserRoleCase.BoundaryDefault),
        BuildCase(GetListUserRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListUserRoleCase.ValidDefault),
        BuildCase(GetListUserRoleCase.ValidWithoutUserId),
        BuildCase(GetListUserRoleCase.ValidWithoutRoleId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListUserRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListUserRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListUserRoleCase caseId)
    {
        return caseId switch
        {
            GetListUserRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListUserRoleCase.ValidWithoutUserId => CreateCase("valid-without-UserId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListUserRoleCase.ValidWithoutRoleId => CreateCase("valid-without-RoleId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListUserRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListUserRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQuery CreateRequest(GetListUserRoleCase caseId)
    {
        return caseId switch
        {
            GetListUserRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQuery { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListUserRoleCase.ValidWithoutUserId => new global::EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQuery { UserId = null, RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListUserRoleCase.ValidWithoutRoleId => new global::EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQuery { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = null, PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListUserRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQuery { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListUserRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.UserRoles.GetListUserRole.GetListUserRoleQuery { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}