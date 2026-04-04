using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Abstraction;
using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.Logout;

internal enum LogoutCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidPrincipalMissing,
    InvalidUserIdMissing,
    ExceptionPath,
}

internal static class LogoutTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "Logout",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.Logout.LogoutCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/Logout/LogoutCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/Logout/LogoutCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(LogoutCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(LogoutCase.BoundaryDefault),
        BuildCase(LogoutCase.InvalidPrincipalMissing),
        BuildCase(LogoutCase.InvalidUserIdMissing),
        BuildCase(LogoutCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(LogoutCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(LogoutCase.InvalidPrincipalMissing),
        BuildCase(LogoutCase.InvalidUserIdMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(LogoutCase.ExceptionPath)
    ];

    public static IEnumerable<object[]> NormalHandlerCases()
    {
        return NormalCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> NormalValidatorCases()
    {
        return [];
    }

    public static IEnumerable<object[]> HandlerBranchCases()
    {
        return BranchCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> ValidatorBranchCases()
    {
        return [];
    }

    private static UseCaseCaseSet BuildCase(LogoutCase caseId)
    {
        return caseId switch
        {
            LogoutCase.ValidDefault => CreateSuccessCase("valid-default", UseCaseCaseKind.Valid, CreateRequest(caseId), Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")),
            LogoutCase.BoundaryDefault => CreateSuccessCase("boundary-default", UseCaseCaseKind.Boundary, CreateRequest(caseId), Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")),
            LogoutCase.InvalidPrincipalMissing => CreateFailureCase(
                "invalid-principal-missing",
                CreateRequest(caseId),
                mocks =>
                {
                    var jwtTokenServiceMock = mocks.StrictMock<IJwtTokenService>();
                    jwtTokenServiceMock
                        .Setup(service => service.ValidateAccessToken(CreateRequest(caseId).AccessToken, false))
                        .Returns((ClaimsPrincipal?)null);
                }),
            LogoutCase.InvalidUserIdMissing => CreateFailureCase(
                "invalid-userid-missing",
                CreateRequest(caseId),
                mocks =>
                {
                    var jwtTokenServiceMock = mocks.StrictMock<IJwtTokenService>();
                    jwtTokenServiceMock
                        .Setup(service => service.ValidateAccessToken(CreateRequest(caseId).AccessToken, false))
                        .Returns(new ClaimsPrincipal(new ClaimsIdentity(
                        [
                            new Claim("role", "Tutor")
                        ], "TestAuth")));
                }),
            LogoutCase.ExceptionPath => CreateCase(
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

    private static global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand CreateRequest(LogoutCase caseId)
    {
        return caseId switch
        {
            LogoutCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand { AccessToken = "logout-valid-token" },
            LogoutCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand { AccessToken = "logout-boundary-token" },
            LogoutCase.InvalidPrincipalMissing => new global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand { AccessToken = "logout-invalid-principal" },
            LogoutCase.InvalidUserIdMissing => new global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand { AccessToken = "logout-missing-userid" },
            LogoutCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand { AccessToken = "logout-exception" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand request,
        Guid userId)
    {
        var pattern = RedisKeyGenerator.GenerateRefreshTokenKeyDeletePattern(userId);

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
                        var jwtTokenServiceMock = mocks.StrictMock<IJwtTokenService>();
                        var redisServiceMock = mocks.StrictMock<IRedisService>();

                        jwtTokenServiceMock
                            .Setup(service => service.ValidateAccessToken(request.AccessToken, false))
                            .Returns(new ClaimsPrincipal(new ClaimsIdentity(
                            [
                                new Claim("sub", userId.ToString()),
                                new Claim("role", "Tutor")
                            ], "TestAuth")));

                        redisServiceMock
                            .Setup(service => service.DeleteCacheWithPatternAsync(pattern))
                            .Returns(Task.CompletedTask);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Authentication.Logout.LogoutCommand request,
        Action<UseCaseMockContext> arrangeMocks)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = UseCaseCaseKind.Invalid,
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
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("Auth.Logout.InvalidToken", result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }
}
