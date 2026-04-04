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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.UpdatePermission;

internal enum UpdatePermissionCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidPermissionMissing,
    InvalidCodeExistsExisting,
    ExceptionPath,
}

internal static class UpdatePermissionTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdatePermission",
        RequestTypeFullName = "EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Permissions/UpdatePermission/UpdatePermissionCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Permissions/UpdatePermission/UpdatePermissionCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Permissions/UpdatePermission/UpdatePermissionCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdatePermissionCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdatePermissionCase.BoundaryDefault),
        BuildCase(UpdatePermissionCase.InvalidRequestShape),
        BuildCase(UpdatePermissionCase.InvalidPermissionMissing),
        BuildCase(UpdatePermissionCase.InvalidCodeExistsExisting),
        BuildCase(UpdatePermissionCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdatePermissionCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdatePermissionCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdatePermissionCase.InvalidRequestShape),
        BuildCase(UpdatePermissionCase.InvalidPermissionMissing),
        BuildCase(UpdatePermissionCase.InvalidCodeExistsExisting),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdatePermissionCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdatePermissionCase caseId)
    {
        return caseId switch
        {
            UpdatePermissionCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdatePermissionCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdatePermissionCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdatePermissionCase.InvalidPermissionMissing => CreateCase("invalid-permission-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdatePermissionCase.InvalidCodeExistsExisting => CreateDuplicateCodeCase(),
            UpdatePermissionCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand CreateRequest(UpdatePermissionCase caseId)
    {
        return caseId switch
        {
            UpdatePermissionCase.ValidDefault => new global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            UpdatePermissionCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            UpdatePermissionCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Code = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Description = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" },
            UpdatePermissionCase.InvalidPermissionMissing => new global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Code = "TEST-CODE", Description = "Sample description" },
            UpdatePermissionCase.InvalidCodeExistsExisting => new global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            UpdatePermissionCase.ExceptionPath => new global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Code = "TEST-CODE", Description = "Sample description" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateDuplicateCodeCase()
    {
        var request = new global::EngConnect.Application.UseCases.Permissions.UpdatePermission.UpdatePermissionCommand
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Code = "PERMISSION-DUPLICATE",
            Description = "Updated description"
        };

        var currentPermission = new Permission
        {
            Id = request.Id,
            Code = "PERMISSION-CURRENT",
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
                        var repositoryMock = new Mock<IGenericRepository<Permission, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Permission, Guid>())
                            .Returns(repositoryMock.Object);

                        repositoryMock
                            .Setup(repository => repository.FindSingleAsync(
                                It.IsAny<Expression<Func<Permission, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Permission, object>>[]>()))
                            .ReturnsAsync(currentPermission);

                        repositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<Permission, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Permission, object>>[]>()))
                            .ReturnsAsync(true);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("Permission.PermissionAlreadyExists", result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }
}
