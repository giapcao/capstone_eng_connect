using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole;

internal enum DeletePermissionRoleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidPermissionRoleMissing,
    ExceptionPath,
}

internal static class DeletePermissionRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeletePermissionRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole.DeletePermissionRoleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole.DeletePermissionRoleCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/DeletePermissionRole/DeletePermissionRoleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/DeletePermissionRole/DeletePermissionRoleCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeletePermissionRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeletePermissionRoleCase.BoundaryDefault),
        BuildCase(DeletePermissionRoleCase.InvalidPermissionRoleMissing),
        BuildCase(DeletePermissionRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeletePermissionRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeletePermissionRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeletePermissionRoleCase.InvalidPermissionRoleMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeletePermissionRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeletePermissionRoleCase caseId)
    {
        return caseId switch
        {
            DeletePermissionRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeletePermissionRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeletePermissionRoleCase.InvalidPermissionRoleMissing => CreateCase("invalid-permissionRole-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeletePermissionRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole.DeletePermissionRoleCommand CreateRequest(DeletePermissionRoleCase caseId)
    {
        return caseId switch
        {
            DeletePermissionRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole.DeletePermissionRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeletePermissionRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole.DeletePermissionRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeletePermissionRoleCase.InvalidPermissionRoleMissing => new global::EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole.DeletePermissionRoleCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeletePermissionRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole.DeletePermissionRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}