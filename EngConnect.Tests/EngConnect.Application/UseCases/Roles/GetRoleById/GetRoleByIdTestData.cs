using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.GetRoleById;

internal enum GetRoleByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRoleMissing,
    ExceptionPath,
}

internal static class GetRoleByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetRoleById",
        RequestTypeFullName = "EngConnect.Application.UseCases.Roles.GetRoleById.GetRoleByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Roles.GetRoleById.GetRoleByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Roles/GetRoleById/GetRoleByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Roles/GetRoleById/GetRoleByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetRoleByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetRoleByIdCase.BoundaryDefault),
        BuildCase(GetRoleByIdCase.InvalidRoleMissing),
        BuildCase(GetRoleByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetRoleByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetRoleByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetRoleByIdCase.InvalidRoleMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetRoleByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetRoleByIdCase caseId)
    {
        return caseId switch
        {
            GetRoleByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetRoleByIdCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetRoleByIdCase.InvalidRoleMissing => CreateCase("invalid-role-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetRoleByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Roles.GetRoleById.GetRoleByIdQuery CreateRequest(GetRoleByIdCase caseId)
    {
        return caseId switch
        {
            GetRoleByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.Roles.GetRoleById.GetRoleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetRoleByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Roles.GetRoleById.GetRoleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            GetRoleByIdCase.InvalidRoleMissing => new global::EngConnect.Application.UseCases.Roles.GetRoleById.GetRoleByIdQuery(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")),
            GetRoleByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.Roles.GetRoleById.GetRoleByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}