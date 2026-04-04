using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.CreateUserRole;

internal enum CreateUserRoleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidUserExistsMissing,
    InvalidRoleExistsMissing,
    InvalidUserRoleExistsExisting,
    ExceptionPath,
}

internal static class CreateUserRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateUserRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/UserRoles/CreateUserRole/CreateUserRoleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/UserRoles/CreateUserRole/CreateUserRoleCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/UserRoles/CreateUserRole/CreateUserRoleCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateUserRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateUserRoleCase.BoundaryDefault),
        BuildCase(CreateUserRoleCase.InvalidRequestShape),
        BuildCase(CreateUserRoleCase.InvalidUserExistsMissing),
        BuildCase(CreateUserRoleCase.InvalidRoleExistsMissing),
        BuildCase(CreateUserRoleCase.InvalidUserRoleExistsExisting),
        BuildCase(CreateUserRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateUserRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateUserRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateUserRoleCase.InvalidRequestShape),
        BuildCase(CreateUserRoleCase.InvalidUserExistsMissing),
        BuildCase(CreateUserRoleCase.InvalidRoleExistsMissing),
        BuildCase(CreateUserRoleCase.InvalidUserRoleExistsExisting),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateUserRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateUserRoleCase caseId)
    {
        return caseId switch
        {
            CreateUserRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateUserRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateUserRoleCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateUserRoleCase.InvalidUserExistsMissing => CreateCase("invalid-userExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateUserRoleCase.InvalidRoleExistsMissing => CreateCase("invalid-roleExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateUserRoleCase.InvalidUserRoleExistsExisting => CreateCase("invalid-userRoleExists-existing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateUserRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand CreateRequest(CreateUserRoleCase caseId)
    {
        return caseId switch
        {
            CreateUserRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreateUserRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreateUserRoleCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand { UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), RoleId = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            CreateUserRoleCase.InvalidUserExistsMissing => new global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand { UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreateUserRoleCase.InvalidRoleExistsMissing => new global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            CreateUserRoleCase.InvalidUserRoleExistsExisting => new global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            CreateUserRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.UserRoles.CreateUserRole.CreateUserRoleCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}