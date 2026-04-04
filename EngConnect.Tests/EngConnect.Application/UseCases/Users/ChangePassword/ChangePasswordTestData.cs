using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Tests.Common;
using EngConnect.Domain.Persistence.Models;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ChangePassword;

internal enum ChangePasswordCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidUserMissing,
    InvalidOldPassword,
    ExceptionPath,
}

internal static class ChangePasswordTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "ChangePassword",
        RequestTypeFullName = "EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Users/ChangePassword/ChangePasswordCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Users/ChangePassword/ChangePasswordCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Users/ChangePassword/ChangePasswordCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(ChangePasswordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(ChangePasswordCase.BoundaryDefault),
        BuildCase(ChangePasswordCase.InvalidRequestShape),
        BuildCase(ChangePasswordCase.InvalidUserMissing),
        BuildCase(ChangePasswordCase.InvalidOldPassword),
        BuildCase(ChangePasswordCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(ChangePasswordCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(ChangePasswordCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(ChangePasswordCase.InvalidRequestShape),
        BuildCase(ChangePasswordCase.InvalidUserMissing),
        BuildCase(ChangePasswordCase.InvalidOldPassword),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(ChangePasswordCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(ChangePasswordCase caseId)
    {
        return caseId switch
        {
            ChangePasswordCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            ChangePasswordCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            ChangePasswordCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            ChangePasswordCase.InvalidUserMissing => CreateCase("invalid-user-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            ChangePasswordCase.InvalidOldPassword => CreateFailureCase("invalid-old-password", CreateRequest(caseId), oldPasswordMatches: false),
            ChangePasswordCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand request,
        bool oldPasswordMatches)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = UseCaseCaseKind.Invalid,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                            .Returns(userRepositoryMock.Object);

                        userRepositoryMock
                            .Setup(repository => repository.FindSingleAsync(
                                It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(new User
                            {
                                Id = request.UserId,
                                PasswordHash = HashHelper.HashPassword(oldPasswordMatches ? request.OldPassword : "AnotherP@ssw0rd!")
                            });
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("User.InvalidPassword", result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
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

    private static global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand CreateRequest(ChangePasswordCase caseId)
    {
        return caseId switch
        {
            ChangePasswordCase.ValidDefault => new global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), OldPassword = "P@ssw0rd!", NewPassword = "P@ssw0rd!" },
            ChangePasswordCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), OldPassword = "P@ssw0rd!", NewPassword = "P@ssw0rd!" },
            ChangePasswordCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand { UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), OldPassword = "", NewPassword = "" },
            ChangePasswordCase.InvalidUserMissing => new global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand { UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), OldPassword = "P@ssw0rd!", NewPassword = "P@ssw0rd!" },
            ChangePasswordCase.InvalidOldPassword => new global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), OldPassword = "WrongP@ssw0rd!", NewPassword = "P@ssw0rd!New" },
            ChangePasswordCase.ExceptionPath => new global::EngConnect.Application.UseCases.Users.ChangePassword.ChangePasswordCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), OldPassword = "P@ssw0rd!", NewPassword = "P@ssw0rd!" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
