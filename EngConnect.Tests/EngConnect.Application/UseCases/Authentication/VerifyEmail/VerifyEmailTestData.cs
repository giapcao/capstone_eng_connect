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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyEmail;

internal enum VerifyEmailCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidTokenMissing,
    InvalidStoredUserIdFormat,
    InvalidUserMissing,
    ExceptionPath,
}

internal static class VerifyEmailTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "VerifyEmail",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/VerifyEmail/VerifyEmailCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/VerifyEmail/VerifyEmailCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Authentication/VerifyEmail/VerifyEmailCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(VerifyEmailCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(VerifyEmailCase.BoundaryDefault),
        BuildCase(VerifyEmailCase.InvalidRequestShape),
        BuildCase(VerifyEmailCase.InvalidTokenMissing),
        BuildCase(VerifyEmailCase.InvalidStoredUserIdFormat),
        BuildCase(VerifyEmailCase.InvalidUserMissing),
        BuildCase(VerifyEmailCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(VerifyEmailCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(VerifyEmailCase.InvalidRequestShape),
        BuildCase(VerifyEmailCase.InvalidTokenMissing),
        BuildCase(VerifyEmailCase.InvalidStoredUserIdFormat),
        BuildCase(VerifyEmailCase.InvalidUserMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(VerifyEmailCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(VerifyEmailCase caseId)
    {
        return caseId switch
        {
            VerifyEmailCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                storedUserId: "\"77777777-7777-7777-7777-777777777777\""),
            VerifyEmailCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                storedUserId: "88888888-8888-8888-8888-888888888888"),
            VerifyEmailCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            VerifyEmailCase.InvalidTokenMissing => CreateFailureCase(
                "invalid-token-missing",
                CreateRequest(caseId),
                mocks =>
                {
                    var request = CreateRequest(caseId);
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    redisServiceMock
                        .Setup(service => service.GetCacheAsync(RedisKeyGenerator.GenerateEmailVerificationTokenAsKey(request.Token)))
                        .ReturnsAsync((string?)null);
                }),
            VerifyEmailCase.InvalidStoredUserIdFormat => CreateFailureCase(
                "invalid-stored-userid-format",
                CreateRequest(caseId),
                mocks =>
                {
                    var request = CreateRequest(caseId);
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    redisServiceMock
                        .Setup(service => service.GetCacheAsync(RedisKeyGenerator.GenerateEmailVerificationTokenAsKey(request.Token)))
                        .ReturnsAsync("\"not-a-guid\"");
                }),
            VerifyEmailCase.InvalidUserMissing => CreateUserMissingCase(),
            VerifyEmailCase.ExceptionPath => CreateCase(
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

    private static global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand CreateRequest(VerifyEmailCase caseId)
    {
        return caseId switch
        {
            VerifyEmailCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand
            {
                Token = "verify-email-valid"
            },
            VerifyEmailCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand
            {
                Token = "verify-email-boundary"
            },
            VerifyEmailCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand
            {
                Token = string.Empty
            },
            VerifyEmailCase.InvalidTokenMissing => new global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand
            {
                Token = "verify-email-token-missing"
            },
            VerifyEmailCase.InvalidStoredUserIdFormat => new global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand
            {
                Token = "verify-email-invalid-guid"
            },
            VerifyEmailCase.InvalidUserMissing => new global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand
            {
                Token = "verify-email-user-missing"
            },
            VerifyEmailCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand
            {
                Token = "verify-email-exception"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand request,
        string storedUserId)
    {
        var parsedUserId = Guid.Parse(storedUserId.Trim('"'));
        var user = new User
        {
            Id = parsedUserId,
            Email = "verify@example.com",
            FirstName = "Verify",
            LastName = "User",
            IsEmailVerified = false
        };
        var tokenKey = RedisKeyGenerator.GenerateEmailVerificationTokenAsKey(request.Token);

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
                            .Setup(service => service.GetCacheAsync(tokenKey))
                            .ReturnsAsync(storedUserId);
                        redisServiceMock
                            .Setup(service => service.DeleteCacheAsync(tokenKey))
                            .ReturnsAsync(true);

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
                        Assert.True(user.IsEmailVerified);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateUserMissingCase()
    {
        var request = CreateRequest(VerifyEmailCase.InvalidUserMissing);
        var tokenKey = RedisKeyGenerator.GenerateEmailVerificationTokenAsKey(request.Token);

        return CreateFailureCase(
            "invalid-user-missing",
            request,
            mocks =>
            {
                var redisServiceMock = mocks.StrictMock<IRedisService>();
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                redisServiceMock
                    .Setup(service => service.GetCacheAsync(tokenKey))
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
        global::EngConnect.Application.UseCases.Authentication.VerifyEmail.VerifyEmailCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest,
        string expectedErrorCode = "Auth.InvalidOrExpiredEmailVerificationToken")
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
