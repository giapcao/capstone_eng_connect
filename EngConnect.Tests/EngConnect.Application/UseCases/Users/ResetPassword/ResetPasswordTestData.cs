using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ResetPassword;

internal enum ResetPasswordCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTokenMissing,
    InvalidStoredUserIdFormat,
    InvalidUserMissing,
    ExceptionPath,
}

internal static class ResetPasswordTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "ResetPassword",
        RequestTypeFullName = "EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Users/ResetPassword/ResetPasswordCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Users/ResetPassword/ResetPasswordCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Users/ResetPassword/ResetPasswordCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(ResetPasswordCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(ResetPasswordCase.BoundaryDefault),
        BuildCase(ResetPasswordCase.InvalidRequestShape),
        BuildCase(ResetPasswordCase.InvalidTokenMissing),
        BuildCase(ResetPasswordCase.InvalidStoredUserIdFormat),
        BuildCase(ResetPasswordCase.InvalidUserMissing),
        BuildCase(ResetPasswordCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(ResetPasswordCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(ResetPasswordCase.InvalidRequestShape),
        BuildCase(ResetPasswordCase.InvalidTokenMissing),
        BuildCase(ResetPasswordCase.InvalidStoredUserIdFormat),
        BuildCase(ResetPasswordCase.InvalidUserMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(ResetPasswordCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(ResetPasswordCase caseId)
    {
        return caseId switch
        {
            ResetPasswordCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                storedUserId: "\"77777777-7777-7777-7777-777777777777\""),
            ResetPasswordCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                storedUserId: "88888888-8888-8888-8888-888888888888"),
            ResetPasswordCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            ResetPasswordCase.InvalidTokenMissing => CreateFailureCase(
                "invalid-token-missing",
                CreateRequest(caseId),
                mocks =>
                {
                    var request = CreateRequest(caseId);
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    redisServiceMock
                        .Setup(service => service.GetCacheAsync(RedisKeyGenerator.GenerateEmailResetPasswordTokenAsKey(request.Token)))
                        .ReturnsAsync((string?)null);
                }),
            ResetPasswordCase.InvalidStoredUserIdFormat => CreateFailureCase(
                "invalid-stored-userid-format",
                CreateRequest(caseId),
                mocks =>
                {
                    var request = CreateRequest(caseId);
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    redisServiceMock
                        .Setup(service => service.GetCacheAsync(RedisKeyGenerator.GenerateEmailResetPasswordTokenAsKey(request.Token)))
                        .ReturnsAsync("\"not-a-guid\"");
                }),
            ResetPasswordCase.InvalidUserMissing => CreateUserMissingCase(),
            ResetPasswordCase.ExceptionPath => CreateCase(
                "exception-path",
                UseCaseCaseKind.Exception,
                UseCaseHandlerExpectation.Exception,
                UseCaseValidatorExpectation.Skip,
                CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand CreateRequest(ResetPasswordCase caseId)
    {
        return caseId switch
        {
            ResetPasswordCase.ValidDefault => new global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand
            {
                Token = "reset-password-valid",
                NewPassword = "P@ssw0rd!"
            },
            ResetPasswordCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand
            {
                Token = "reset-password-boundary",
                NewPassword = "P@ssw0rd!"
            },
            ResetPasswordCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand
            {
                Token = string.Empty,
                NewPassword = string.Empty
            },
            ResetPasswordCase.InvalidTokenMissing => new global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand
            {
                Token = "reset-password-token-missing",
                NewPassword = "P@ssw0rd!"
            },
            ResetPasswordCase.InvalidStoredUserIdFormat => new global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand
            {
                Token = "reset-password-invalid-guid",
                NewPassword = "P@ssw0rd!"
            },
            ResetPasswordCase.InvalidUserMissing => new global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand
            {
                Token = "reset-password-user-missing",
                NewPassword = "P@ssw0rd!"
            },
            ResetPasswordCase.ExceptionPath => new global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand
            {
                Token = "reset-password-exception",
                NewPassword = "P@ssw0rd!"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand request,
        string storedUserId)
    {
        var parsedUserId = Guid.Parse(storedUserId.Trim('"'));
        var user = new User
        {
            Id = parsedUserId,
            Email = "reset@example.com",
            FirstName = "Reset",
            LastName = "User",
            PasswordHash = "old-hash"
        };
        var redisKey = RedisKeyGenerator.GenerateEmailResetPasswordTokenAsKey(request.Token);
        var refreshTokenPattern = RedisKeyGenerator.GenerateRefreshTokenKeyDeletePattern(user.Id);

        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = UseCaseHandlerExpectation.Completes,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var redisServiceMock = mocks.StrictMock<IRedisService>();
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                        redisServiceMock
                            .Setup(service => service.GetCacheAsync(redisKey))
                            .ReturnsAsync(storedUserId);
                        redisServiceMock
                            .Setup(service => service.DeleteCacheAsync(redisKey))
                            .ReturnsAsync(true);
                        redisServiceMock
                            .Setup(service => service.DeleteCacheWithPatternAsync(refreshTokenPattern))
                            .Returns(Task.CompletedTask);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                            .Returns(userRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);

                        userRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<Expression<Func<User, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(user);
                        userRepositoryMock
                            .Setup(repository => repository.Update(user));
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotEqual("old-hash", user.PasswordHash);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateUserMissingCase()
    {
        var request = CreateRequest(ResetPasswordCase.InvalidUserMissing);
        var redisKey = RedisKeyGenerator.GenerateEmailResetPasswordTokenAsKey(request.Token);

        return CreateFailureCase(
            "invalid-user-missing",
            request,
            mocks =>
            {
                var redisServiceMock = mocks.StrictMock<IRedisService>();
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                redisServiceMock
                    .Setup(service => service.GetCacheAsync(redisKey))
                    .ReturnsAsync("\"99999999-9999-9999-9999-999999999999\"");

                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                    .Returns(userRepositoryMock.Object);

                userRepositoryMock
                    .Setup(repository => repository.FindFirstAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<User, object>>[]>()))
                    .ReturnsAsync((User?)null);
            },
            expectedStatusCode: HttpStatusCode.NotFound,
            expectedErrorCode: "User.NotFound");
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Users.ResetPassword.ResetPasswordCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest,
        string expectedErrorCode = "User.InvalidOrExpiredPasswordResetToken")
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
                    ArrangeMocks = arrangeMocks,
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(expectedStatusCode, result.HttpStatusCode);
                        Assert.Equal(expectedErrorCode, result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }
}
