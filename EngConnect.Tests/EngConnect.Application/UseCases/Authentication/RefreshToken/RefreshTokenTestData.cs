using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RefreshToken;

internal enum RefreshTokenCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidPrincipalMissing,
    InvalidUserIdMissing,
    InvalidStoredTokenMissing,
    InvalidUserMissing,
    ExceptionPath,
}

internal static class RefreshTokenTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "RefreshToken",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RefreshToken/RefreshTokenCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RefreshToken/RefreshTokenCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RefreshToken/RefreshTokenCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(RefreshTokenCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(RefreshTokenCase.BoundaryDefault),
        BuildCase(RefreshTokenCase.InvalidRequestShape),
        BuildCase(RefreshTokenCase.InvalidPrincipalMissing),
        BuildCase(RefreshTokenCase.InvalidUserIdMissing),
        BuildCase(RefreshTokenCase.InvalidStoredTokenMissing),
        BuildCase(RefreshTokenCase.InvalidUserMissing),
        BuildCase(RefreshTokenCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(RefreshTokenCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(RefreshTokenCase.InvalidRequestShape),
        BuildCase(RefreshTokenCase.InvalidPrincipalMissing),
        BuildCase(RefreshTokenCase.InvalidUserIdMissing),
        BuildCase(RefreshTokenCase.InvalidStoredTokenMissing),
        BuildCase(RefreshTokenCase.InvalidUserMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(RefreshTokenCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(RefreshTokenCase caseId)
    {
        return caseId switch
        {
            RefreshTokenCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                wrapStoredTokenInQuotes: false),
            RefreshTokenCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                wrapStoredTokenInQuotes: true),
            RefreshTokenCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            RefreshTokenCase.InvalidPrincipalMissing => CreatePrincipalMissingCase(),
            RefreshTokenCase.InvalidUserIdMissing => CreateUserIdMissingCase(),
            RefreshTokenCase.InvalidStoredTokenMissing => CreateStoredTokenMissingCase(),
            RefreshTokenCase.InvalidUserMissing => CreateUserMissingCase(),
            RefreshTokenCase.ExceptionPath => CreateCase(
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

    private static global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand CreateRequest(RefreshTokenCase caseId)
    {
        return caseId switch
        {
            RefreshTokenCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = "access-token-valid",
                RefreshToken = "refresh-token-valid"
            },
            RefreshTokenCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = "access-token-boundary",
                RefreshToken = "refresh-token-boundary"
            },
            RefreshTokenCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            },
            RefreshTokenCase.InvalidPrincipalMissing => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = "access-token-invalid-principal",
                RefreshToken = "refresh-token-invalid-principal"
            },
            RefreshTokenCase.InvalidUserIdMissing => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = "access-token-missing-userid",
                RefreshToken = "refresh-token-missing-userid"
            },
            RefreshTokenCase.InvalidStoredTokenMissing => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = "access-token-missing-store",
                RefreshToken = "refresh-token-missing-store"
            },
            RefreshTokenCase.InvalidUserMissing => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = "access-token-user-missing",
                RefreshToken = "refresh-token-user-missing"
            },
            RefreshTokenCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand
            {
                AccessToken = "access-token-exception",
                RefreshToken = "refresh-token-exception"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand request,
        bool wrapStoredTokenInQuotes)
    {
        var userId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var principal = CreatePrincipal(userId, "Tutor");
        var user = new User
        {
            Id = userId,
            Email = "refresh@example.com",
            FirstName = "Refresh",
            LastName = "User",
            Status = nameof(UserStatus.Active)
        };
        var expectedAccessToken = $"{name}-new-access-token";
        var expectedRefreshToken = $"{name}-new-refresh-token";
        var currentRefreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(userId, request.RefreshToken);
        var newRefreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(userId, expectedRefreshToken);

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
                        var jwtServiceMock = mocks.StrictMock<IJwtTokenService>();
                        var redisServiceMock = mocks.StrictMock<IRedisService>();
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                        jwtServiceMock
                            .Setup(service => service.ValidateAccessToken(request.AccessToken, false))
                            .Returns(principal);
                        jwtServiceMock
                            .Setup(service => service.GenerateAccessToken(user))
                            .Returns(expectedAccessToken);
                        jwtServiceMock
                            .Setup(service => service.GenerateRefreshToken())
                            .Returns(expectedRefreshToken);

                        redisServiceMock
                            .Setup(service => service.GetCacheAsync(currentRefreshTokenKey))
                            .ReturnsAsync(wrapStoredTokenInQuotes ? $"\"{request.RefreshToken}\"" : request.RefreshToken);
                        redisServiceMock
                            .Setup(service => service.DeleteCacheAsync(currentRefreshTokenKey))
                            .ReturnsAsync(true);
                        redisServiceMock
                            .Setup(service => service.SetCacheAsync(
                                newRefreshTokenKey,
                                expectedRefreshToken,
                                It.IsAny<TimeSpan?>(),
                                true))
                            .ReturnsAsync(true);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                            .Returns(userRepositoryMock.Object);

                        userRepositoryMock
                            .Setup(repository => repository.FindSingleAsync(
                                It.IsAny<Expression<Func<User, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(user);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<RefreshTokenResponse>>(resultObject);
                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.Equal(expectedAccessToken, result.Data?.AccessToken);
                        Assert.Equal(expectedRefreshToken, result.Data?.RefreshToken);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreatePrincipalMissingCase()
    {
        var request = CreateRequest(RefreshTokenCase.InvalidPrincipalMissing);

        return CreateFailureCase(
            "invalid-principal-missing",
            request,
            arrangeMocks: mocks =>
            {
                var jwtServiceMock = mocks.StrictMock<IJwtTokenService>();
                jwtServiceMock
                    .Setup(service => service.ValidateAccessToken(request.AccessToken, false))
                    .Returns((ClaimsPrincipal?)null);
            });
    }

    private static UseCaseCaseSet CreateUserIdMissingCase()
    {
        var request = CreateRequest(RefreshTokenCase.InvalidUserIdMissing);

        return CreateFailureCase(
            "invalid-userid-missing",
            request,
            arrangeMocks: mocks =>
            {
                var jwtServiceMock = mocks.StrictMock<IJwtTokenService>();
                jwtServiceMock
                    .Setup(service => service.ValidateAccessToken(request.AccessToken, false))
                    .Returns(new ClaimsPrincipal(new ClaimsIdentity(
                    [
                        new Claim("role", "Tutor")
                    ], "TestAuth")));
            });
    }

    private static UseCaseCaseSet CreateStoredTokenMissingCase()
    {
        var request = CreateRequest(RefreshTokenCase.InvalidStoredTokenMissing);
        var userId = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var refreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(userId, request.RefreshToken);

        return CreateFailureCase(
            "invalid-stored-token-missing",
            request,
            arrangeMocks: mocks =>
            {
                var jwtServiceMock = mocks.StrictMock<IJwtTokenService>();
                var redisServiceMock = mocks.StrictMock<IRedisService>();

                jwtServiceMock
                    .Setup(service => service.ValidateAccessToken(request.AccessToken, false))
                    .Returns(CreatePrincipal(userId, "Tutor"));

                redisServiceMock
                    .Setup(service => service.GetCacheAsync(refreshTokenKey))
                    .ReturnsAsync((string?)null);
            });
    }

    private static UseCaseCaseSet CreateUserMissingCase()
    {
        var request = CreateRequest(RefreshTokenCase.InvalidUserMissing);
        var userId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var refreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(userId, request.RefreshToken);

        return CreateFailureCase(
            "invalid-user-missing",
            request,
            arrangeMocks: mocks =>
            {
                var jwtServiceMock = mocks.StrictMock<IJwtTokenService>();
                var redisServiceMock = mocks.StrictMock<IRedisService>();
                var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                jwtServiceMock
                    .Setup(service => service.ValidateAccessToken(request.AccessToken, false))
                    .Returns(CreatePrincipal(userId, "Tutor"));

                redisServiceMock
                    .Setup(service => service.GetCacheAsync(refreshTokenKey))
                    .ReturnsAsync(request.RefreshToken);

                unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                    .Returns(userRepositoryMock.Object);

                userRepositoryMock
                    .Setup(repository => repository.FindSingleAsync(
                        It.IsAny<Expression<Func<User, bool>>>(),
                        It.IsAny<bool>(),
                        It.IsAny<CancellationToken>(),
                        It.IsAny<Expression<Func<User, object>>[]>()))
                    .ReturnsAsync((User?)null);
            },
            expectedErrorCode: "User.NotFound");
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Authentication.RefreshToken.RefreshTokenCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        string expectedErrorCode = "Auth.RefreshToken.InvalidToken",
        HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest)
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
                        var result = Assert.IsType<Result<RefreshTokenResponse>>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(expectedStatusCode, result.HttpStatusCode);
                        Assert.Equal(expectedErrorCode, result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static ClaimsPrincipal CreatePrincipal(Guid userId, string role)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("sub", userId.ToString()),
            new Claim("role", role)
        ], "TestAuth"));
    }
}
