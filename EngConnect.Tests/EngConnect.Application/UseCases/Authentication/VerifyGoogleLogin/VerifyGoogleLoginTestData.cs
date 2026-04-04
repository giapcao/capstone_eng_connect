using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin;

internal enum VerifyGoogleLoginCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestGuardToken,
    InvalidTokenMissingInCache,
    ExceptionPath,
}

internal static class VerifyGoogleLoginTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "VerifyGoogleLogin",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/VerifyGoogleLogin/VerifyGoogleLoginCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/VerifyGoogleLogin/VerifyGoogleLoginCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(VerifyGoogleLoginCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(VerifyGoogleLoginCase.BoundaryDefault),
        BuildCase(VerifyGoogleLoginCase.InvalidRequestGuardToken),
        BuildCase(VerifyGoogleLoginCase.InvalidTokenMissingInCache),
        BuildCase(VerifyGoogleLoginCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(VerifyGoogleLoginCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(VerifyGoogleLoginCase.InvalidRequestGuardToken),
        BuildCase(VerifyGoogleLoginCase.InvalidTokenMissingInCache)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(VerifyGoogleLoginCase.ExceptionPath)
    ];

    public static IEnumerable<object[]> NormalHandlerCases()
    {
        return NormalCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> NormalValidatorCases()
    {
        return Array.Empty<object[]>();
    }

    public static IEnumerable<object[]> HandlerBranchCases()
    {
        return BranchCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> ValidatorBranchCases()
    {
        return Array.Empty<object[]>();
    }

    private static UseCaseCaseSet BuildCase(VerifyGoogleLoginCase caseId)
    {
        return caseId switch
        {
            VerifyGoogleLoginCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                CreateLoginResponse("valid-default")),
            VerifyGoogleLoginCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                CreateLoginResponse("boundary-default")),
            VerifyGoogleLoginCase.InvalidRequestGuardToken => CreateFailureCase(
                "invalid-request-guard-token",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: _ => { },
                expectedStatusCode: HttpStatusCode.BadRequest,
                expectedErrorCode: "Auth.GoogleOAuth.InvalidToken"),
            VerifyGoogleLoginCase.InvalidTokenMissingInCache => CreateFailureCase(
                "invalid-token-missing-in-cache",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    redisServiceMock
                        .Setup(service => service.GetCacheAsync<UserLoginResponse>(RedisKeyGenerator.GenerateUserLoginTokenKey("google-login-token-missing")))
                        .ReturnsAsync((UserLoginResponse?)null);
                },
                expectedStatusCode: HttpStatusCode.BadRequest,
                expectedErrorCode: "Auth.GoogleOAuth.InvalidToken"),
            VerifyGoogleLoginCase.ExceptionPath => CreateFailureCase(
                "exception-redis-read-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    redisServiceMock
                        .Setup(service => service.GetCacheAsync<UserLoginResponse>(RedisKeyGenerator.GenerateUserLoginTokenKey("google-login-token-exception")))
                        .ThrowsAsync(new InvalidOperationException("redis read failed"));
                },
                expectedStatusCode: HttpStatusCode.InternalServerError,
                expectedErrorCode: "Server.Error"),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand request,
        UserLoginResponse loginResponse)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = UseCaseHandlerExpectation.Completes,
            ValidatorExpectation = UseCaseValidatorExpectation.Skip,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var redisServiceMock = mocks.StrictMock<IRedisService>();
                        var cacheKey = RedisKeyGenerator.GenerateUserLoginTokenKey(request.Token!);

                        redisServiceMock
                            .Setup(service => service.GetCacheAsync<UserLoginResponse>(cacheKey))
                            .ReturnsAsync(loginResponse);
                        redisServiceMock
                            .Setup(service => service.DeleteCacheAsync(cacheKey))
                            .ReturnsAsync(true);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<UserLoginResponse>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.Equal(loginResponse.AccessToken, result.Data!.AccessToken);
                        Assert.Equal(loginResponse.RefreshToken, result.Data.RefreshToken);
                        Assert.Equal(loginResponse.AvatarUrl, result.Data.AvatarUrl);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        HttpStatusCode expectedStatusCode,
        string expectedErrorCode)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Skip,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = arrangeMocks,
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<UserLoginResponse>>(resultObject);

                        Assert.True(result.IsFailure);
                        Assert.Equal(expectedStatusCode, result.HttpStatusCode);
                        Assert.Equal(expectedErrorCode, result.Error?.Code);
                        Assert.Null(result.Data);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand CreateRequest(VerifyGoogleLoginCase caseId)
    {
        return caseId switch
        {
            VerifyGoogleLoginCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand
            {
                Token = "google-login-token-valid"
            },
            VerifyGoogleLoginCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand
            {
                Token = "g"
            },
            VerifyGoogleLoginCase.InvalidRequestGuardToken => new global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand
            {
                Token = string.Empty
            },
            VerifyGoogleLoginCase.InvalidTokenMissingInCache => new global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand
            {
                Token = "google-login-token-missing"
            },
            VerifyGoogleLoginCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin.VerifyGoogleLoginCommand
            {
                Token = "google-login-token-exception"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UserLoginResponse CreateLoginResponse(string suffix)
    {
        return new UserLoginResponse
        {
            FirstName = "Google",
            LastName = "User",
            Username = $"google-{suffix}",
            Roles = ["Student"],
            AvatarUrl = $"https://files.example/{suffix}.png",
            AccessToken = $"access-{suffix}",
            RefreshToken = $"refresh-{suffix}"
        };
    }
}
