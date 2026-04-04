using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole;

internal enum GetListPermissionRoleCase
{
    ValidDefault,
    ValidWithoutPermissionId,
    ValidWithoutRoleId,
    BoundaryDefault,
    ExceptionPath,
}

internal static class GetListPermissionRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListPermissionRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/GetListPermissionRole/GetListPermissionRoleQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/GetListPermissionRole/GetListPermissionRoleQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListPermissionRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListPermissionRoleCase.ValidWithoutPermissionId),
        BuildCase(GetListPermissionRoleCase.ValidWithoutRoleId),
        BuildCase(GetListPermissionRoleCase.BoundaryDefault),
        BuildCase(GetListPermissionRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetListPermissionRoleCase.ValidDefault),
        BuildCase(GetListPermissionRoleCase.ValidWithoutPermissionId),
        BuildCase(GetListPermissionRoleCase.ValidWithoutRoleId),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListPermissionRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [

    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListPermissionRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetListPermissionRoleCase caseId)
    {
        return caseId switch
        {
            GetListPermissionRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListPermissionRoleCase.ValidWithoutPermissionId => CreateCase("valid-without-PermissionId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListPermissionRoleCase.ValidWithoutRoleId => CreateCase("valid-without-RoleId", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListPermissionRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetListPermissionRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQuery CreateRequest(GetListPermissionRoleCase caseId)
    {
        return caseId switch
        {
            GetListPermissionRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQuery { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListPermissionRoleCase.ValidWithoutPermissionId => new global::EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQuery { PermissionId = null, RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListPermissionRoleCase.ValidWithoutRoleId => new global::EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQuery { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = null, PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListPermissionRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQuery { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetListPermissionRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole.GetListPermissionRoleQuery { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}