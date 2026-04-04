using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.UpdateRole;

internal enum UpdateRoleCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidRoleMissing,
    InvalidCodeExistsExisting,
    ExceptionPath,
}

internal static class UpdateRoleTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateRole",
        RequestTypeFullName = "EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Roles/UpdateRole/UpdateRoleCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Roles/UpdateRole/UpdateRoleCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Roles/UpdateRole/UpdateRoleCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateRoleCase.BoundaryDefault),
        BuildCase(UpdateRoleCase.InvalidRequestShape),
        BuildCase(UpdateRoleCase.InvalidRoleMissing),
        BuildCase(UpdateRoleCase.InvalidCodeExistsExisting),
        BuildCase(UpdateRoleCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateRoleCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateRoleCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateRoleCase.InvalidRequestShape),
        BuildCase(UpdateRoleCase.InvalidRoleMissing),
        BuildCase(UpdateRoleCase.InvalidCodeExistsExisting),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateRoleCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateRoleCase caseId)
    {
        return caseId switch
        {
            UpdateRoleCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateRoleCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateRoleCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateRoleCase.InvalidRoleMissing => CreateCase("invalid-role-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateRoleCase.InvalidCodeExistsExisting => CreateDuplicateCodeCase(),
            UpdateRoleCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand CreateRequest(UpdateRoleCase caseId)
    {
        return caseId switch
        {
            UpdateRoleCase.ValidDefault => new global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            UpdateRoleCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            UpdateRoleCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Code = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Description = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" },
            UpdateRoleCase.InvalidRoleMissing => new global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Code = "TEST-CODE", Description = "Sample description" },
            UpdateRoleCase.InvalidCodeExistsExisting => new global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            UpdateRoleCase.ExceptionPath => new global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateDuplicateCodeCase()
    {
        var request = new global::EngConnect.Application.UseCases.Roles.UpdateRole.UpdateRoleCommand
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Code = "ROLE-DUPLICATE",
            Description = "Updated description"
        };

        var currentRole = new Role
        {
            Id = request.Id,
            Code = "ROLE-CURRENT",
            Description = "Current description"
        };

        return new UseCaseCaseSet
        {
            Name = "invalid-codeExists-existing",
            Kind = UseCaseCaseKind.Invalid,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = "invalid-codeExists-existing",
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var repositoryMock = new Mock<IGenericRepository<Role, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Role, Guid>())
                            .Returns(repositoryMock.Object);

                        repositoryMock
                            .Setup(repository => repository.FindSingleAsync(
                                It.IsAny<Expression<Func<Role, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Role, object>>[]>()))
                            .ReturnsAsync(currentRole);

                        repositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<Role, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Role, object>>[]>()))
                            .ReturnsAsync(true);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("Role.RoleAlreadyExists", result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }
}
