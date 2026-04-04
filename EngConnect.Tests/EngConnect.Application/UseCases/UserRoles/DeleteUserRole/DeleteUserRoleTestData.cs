using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.DeleteUserRole;

internal enum DeleteUserRoleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidUserRoleMissing,
    ExceptionPath,
}

internal static class DeleteUserRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeleteUserRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.UserRoles.DeleteUserRole.DeleteUserRoleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.UserRoles.DeleteUserRole.DeleteUserRoleCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/UserRoles/DeleteUserRole/DeleteUserRoleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/UserRoles/DeleteUserRole/DeleteUserRoleCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeleteUserRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeleteUserRoleCase.BoundaryDefault),
        BuildCase(DeleteUserRoleCase.InvalidUserRoleMissing),
        BuildCase(DeleteUserRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeleteUserRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeleteUserRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeleteUserRoleCase.InvalidUserRoleMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeleteUserRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeleteUserRoleCase caseId)
    {
        return caseId switch
        {
            DeleteUserRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteUserRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteUserRoleCase.InvalidUserRoleMissing => CreateCase("invalid-userRole-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeleteUserRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.UserRoles.DeleteUserRole.DeleteUserRoleCommand CreateRequest(DeleteUserRoleCase caseId)
    {
        return caseId switch
        {
            DeleteUserRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.UserRoles.DeleteUserRole.DeleteUserRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteUserRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.UserRoles.DeleteUserRole.DeleteUserRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeleteUserRoleCase.InvalidUserRoleMissing => new global::EngConnect.Application.UseCases.UserRoles.DeleteUserRole.DeleteUserRoleCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeleteUserRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.UserRoles.DeleteUserRole.DeleteUserRoleCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}