using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.DeleteRole;

internal enum DeleteRoleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRoleMissing,
    ExceptionPath,
}

internal static class DeleteRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.Roles.DeleteRole.DeleteRoleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Roles.DeleteRole.DeleteRoleCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Roles/DeleteRole/DeleteRoleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Roles/DeleteRole/DeleteRoleCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteRoleCase.BoundaryDefault),
        BuildCase(DeleteRoleCase.InvalidRoleMissing),
        BuildCase(DeleteRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteRoleCase.InvalidRoleMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteRoleCase caseId)
    {
        return caseId switch
        {
            DeleteRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteRoleCase.InvalidRoleMissing => CreateCase("invalid-role-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Roles.DeleteRole.DeleteRoleCommand CreateRequest(DeleteRoleCase caseId)
    {
        return caseId switch
        {
            DeleteRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.Roles.DeleteRole.DeleteRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Roles.DeleteRole.DeleteRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteRoleCase.InvalidRoleMissing => new global::EngConnect.Application.UseCases.Roles.DeleteRole.DeleteRoleCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeleteRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.Roles.DeleteRole.DeleteRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}