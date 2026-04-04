using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById;

internal enum GetPermissionRoleByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class GetPermissionRoleByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetPermissionRoleById",
        RequestTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById.GetPermissionRoleByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById.GetPermissionRoleByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/GetPermissionRoleById/GetPermissionRoleByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/GetPermissionRoleById/GetPermissionRoleByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetPermissionRoleByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetPermissionRoleByIdCase.BoundaryDefault),
        BuildCase(GetPermissionRoleByIdCase.InvalidNotFoundOrNull),
        BuildCase(GetPermissionRoleByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetPermissionRoleByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetPermissionRoleByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetPermissionRoleByIdCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetPermissionRoleByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetPermissionRoleByIdCase caseId)
    {
        return caseId switch
        {
            GetPermissionRoleByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetPermissionRoleByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetPermissionRoleByIdCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            GetPermissionRoleByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById.GetPermissionRoleByIdQuery CreateRequest(GetPermissionRoleByIdCase caseId)
    {
        return caseId switch
        {
            GetPermissionRoleByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById.GetPermissionRoleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetPermissionRoleByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById.GetPermissionRoleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetPermissionRoleByIdCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById.GetPermissionRoleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetPermissionRoleByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById.GetPermissionRoleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}