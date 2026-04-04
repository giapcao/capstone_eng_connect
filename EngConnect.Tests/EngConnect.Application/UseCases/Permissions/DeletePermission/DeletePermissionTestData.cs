using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.DeletePermission;

internal enum DeletePermissionCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidPermissionMissing,
    ExceptionPath,
}

internal static class DeletePermissionTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "DeletePermission",
        RequestTypeFullName = "EngConnect.Application.UseCases.Permissions.DeletePermission.DeletePermissionCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Permissions.DeletePermission.DeletePermissionCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Permissions/DeletePermission/DeletePermissionCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Permissions/DeletePermission/DeletePermissionCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(DeletePermissionCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(DeletePermissionCase.BoundaryDefault),
        BuildCase(DeletePermissionCase.InvalidPermissionMissing),
        BuildCase(DeletePermissionCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(DeletePermissionCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(DeletePermissionCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(DeletePermissionCase.InvalidPermissionMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(DeletePermissionCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(DeletePermissionCase caseId)
    {
        return caseId switch
        {
            DeletePermissionCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeletePermissionCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeletePermissionCase.InvalidPermissionMissing => CreateCase("invalid-permission-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            DeletePermissionCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Permissions.DeletePermission.DeletePermissionCommand CreateRequest(DeletePermissionCase caseId)
    {
        return caseId switch
        {
            DeletePermissionCase.ValidDefault => new global::EngConnect.Application.UseCases.Permissions.DeletePermission.DeletePermissionCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeletePermissionCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Permissions.DeletePermission.DeletePermissionCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            DeletePermissionCase.InvalidPermissionMissing => new global::EngConnect.Application.UseCases.Permissions.DeletePermission.DeletePermissionCommand(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            DeletePermissionCase.ExceptionPath => new global::EngConnect.Application.UseCases.Permissions.DeletePermission.DeletePermissionCommand(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}