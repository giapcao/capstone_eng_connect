using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole;

internal enum CreatePermissionRoleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidPermissionExistsMissing,
    InvalidRoleExistsMissing,
    InvalidPermissionRoleExistsExisting,
    ExceptionPath,
}

internal static class CreatePermissionRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreatePermissionRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/CreatePermissionRole/CreatePermissionRoleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/CreatePermissionRole/CreatePermissionRoleCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/PermissionRoles/CreatePermissionRole/CreatePermissionRoleCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreatePermissionRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreatePermissionRoleCase.BoundaryDefault),
        BuildCase(CreatePermissionRoleCase.InvalidRequestShape),
        BuildCase(CreatePermissionRoleCase.InvalidPermissionExistsMissing),
        BuildCase(CreatePermissionRoleCase.InvalidRoleExistsMissing),
        BuildCase(CreatePermissionRoleCase.InvalidPermissionRoleExistsExisting),
        BuildCase(CreatePermissionRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreatePermissionRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreatePermissionRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreatePermissionRoleCase.InvalidRequestShape),
        BuildCase(CreatePermissionRoleCase.InvalidPermissionExistsMissing),
        BuildCase(CreatePermissionRoleCase.InvalidRoleExistsMissing),
        BuildCase(CreatePermissionRoleCase.InvalidPermissionRoleExistsExisting),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreatePermissionRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreatePermissionRoleCase caseId)
    {
        return caseId switch
        {
            CreatePermissionRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreatePermissionRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreatePermissionRoleCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreatePermissionRoleCase.InvalidPermissionExistsMissing => CreateCase("invalid-permissionExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreatePermissionRoleCase.InvalidRoleExistsMissing => CreateCase("invalid-roleExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreatePermissionRoleCase.InvalidPermissionRoleExistsExisting => CreateCase("invalid-permissionRoleExists-existing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreatePermissionRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand CreateRequest(CreatePermissionRoleCase caseId)
    {
        return caseId switch
        {
            CreatePermissionRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreatePermissionRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreatePermissionRoleCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand { PermissionId = Guid.Parse("00000000-0000-0000-0000-000000000000"), RoleId = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            CreatePermissionRoleCase.InvalidPermissionExistsMissing => new global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand { PermissionId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreatePermissionRoleCase.InvalidRoleExistsMissing => new global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            CreatePermissionRoleCase.InvalidPermissionRoleExistsExisting => new global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreatePermissionRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole.CreatePermissionRoleCommand { PermissionId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}