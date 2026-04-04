using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.GetPermissionById;

internal enum GetPermissionByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidPermissionMissing,
    ExceptionPath,
}

internal static class GetPermissionByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetPermissionById",
        RequestTypeFullName = "EngConnect.Application.UseCases.Permissions.GetPermissionById.GetPermissionByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Permissions.GetPermissionById.GetPermissionByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Permissions/GetPermissionById/GetPermissionByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Permissions/GetPermissionById/GetPermissionByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetPermissionByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetPermissionByIdCase.BoundaryDefault),
        BuildCase(GetPermissionByIdCase.InvalidPermissionMissing),
        BuildCase(GetPermissionByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetPermissionByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetPermissionByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetPermissionByIdCase.InvalidPermissionMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetPermissionByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetPermissionByIdCase caseId)
    {
        return caseId switch
        {
            GetPermissionByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetPermissionByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetPermissionByIdCase.InvalidPermissionMissing => CreateCase("invalid-permission-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetPermissionByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Permissions.GetPermissionById.GetPermissionByIdQuery CreateRequest(GetPermissionByIdCase caseId)
    {
        return caseId switch
        {
            GetPermissionByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.Permissions.GetPermissionById.GetPermissionByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetPermissionByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Permissions.GetPermissionById.GetPermissionByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetPermissionByIdCase.InvalidPermissionMissing => new global::EngConnect.Application.UseCases.Permissions.GetPermissionById.GetPermissionByIdQuery(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            GetPermissionByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.Permissions.GetPermissionById.GetPermissionByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}