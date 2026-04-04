using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginByUser;

internal enum LoginByUserCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidUserMissing,
    InvalidPassword,
    ExceptionPath,
}

internal static class LoginByUserTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "LoginByUser",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/LoginByUser/LoginByUserCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/LoginByUser/LoginByUserCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Authentication/LoginByUser/LoginByUserCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(LoginByUserCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(LoginByUserCase.BoundaryDefault),
        BuildCase(LoginByUserCase.InvalidRequestShape),
        BuildCase(LoginByUserCase.InvalidUserMissing),
        BuildCase(LoginByUserCase.InvalidPassword),
        BuildCase(LoginByUserCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(LoginByUserCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(LoginByUserCase.InvalidRequestShape),
        BuildCase(LoginByUserCase.InvalidUserMissing),
        BuildCase(LoginByUserCase.InvalidPassword)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(LoginByUserCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(LoginByUserCase caseId)
    {
        return caseId switch
        {
            LoginByUserCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                CreateUser(hasAvatar: true),
                expectedAvatarUrl: "https://files.example/avatar-student.png"),
            LoginByUserCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                CreateUser(hasAvatar: false),
                expectedAvatarUrl: null),
            LoginByUserCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            LoginByUserCase.InvalidUserMissing => CreateFailureCase(
                "invalid-user-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    mocks.StrictMock<IJwtTokenService>();
                    mocks.StrictMock<IRedisService>();
                    mocks.StrictMock<IAwsStorageService>();

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                        .Returns(userRepositoryMock.Object);

                    userRepositoryMock
                        .Setup(repository => repository.FindAll(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .Returns(new TestAsyncEnumerable<User>(Array.Empty<User>()));
                },
                expectedStatusCode: HttpStatusCode.NotFound,
                expectedErrorCode: "User.NotFound"),
            LoginByUserCase.InvalidPassword => CreateFailureCase(
                "invalid-password",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    mocks.StrictMock<IJwtTokenService>();
                    mocks.StrictMock<IRedisService>();
                    mocks.StrictMock<IAwsStorageService>();

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                        .Returns(userRepositoryMock.Object);

                    userRepositoryMock
                        .Setup(repository => repository.FindAll(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .Returns(new TestAsyncEnumerable<User>([CreateUser(hasAvatar: true, password: "DifferentP@ssw0rd!")]));
                },
                expectedStatusCode: HttpStatusCode.BadRequest,
                expectedErrorCode: "User.InvalidPassword"),
            LoginByUserCase.ExceptionPath => CreateFailureCase(
                "exception-redis-set-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var jwtServiceMock = mocks.StrictMock<IJwtTokenService>();
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    var awsStorageServiceMock = mocks.StrictMock<IAwsStorageService>();
                    var user = CreateUser(hasAvatar: true);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                        .Returns(userRepositoryMock.Object);

                    userRepositoryMock
                        .Setup(repository => repository.FindAll(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .Returns(new TestAsyncEnumerable<User>([user]));

                    jwtServiceMock
                        .Setup(service => service.GenerateAccessToken(user))
                        .Returns("access-token-exception");
                    jwtServiceMock
                        .Setup(service => service.GenerateRefreshToken())
                        .Returns("refresh-token-exception");

                    redisServiceMock
                        .Setup(service => service.DeleteCacheWithPatternAsync(It.IsAny<string>()))
                        .Returns(Task.CompletedTask);
                    redisServiceMock
                        .Setup(service => service.SetCacheAsync(
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<TimeSpan?>(),
                            true))
                        .ThrowsAsync(new InvalidOperationException("redis set failed"));

                    awsStorageServiceMock
                        .Setup(service => service.GetFileUrl("avatars/student.png", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/avatar-student.png");
                },
                expectedStatusCode: HttpStatusCode.InternalServerError,
                expectedErrorCode: "Server.Error"),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand request,
        User user,
        string? expectedAvatarUrl)
    {
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
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                        var jwtServiceMock = mocks.StrictMock<IJwtTokenService>();
                        var redisServiceMock = mocks.StrictMock<IRedisService>();
                        var awsStorageServiceMock = mocks.StrictMock<IAwsStorageService>();

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                            .Returns(userRepositoryMock.Object);

                        userRepositoryMock
                            .Setup(repository => repository.FindAll(
                                It.IsAny<Expression<Func<User, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<User, object>>[]>()))
                            .Returns(new TestAsyncEnumerable<User>([user]));

                        jwtServiceMock
                            .Setup(service => service.GenerateAccessToken(user))
                            .Returns($"access-{name}");
                        jwtServiceMock
                            .Setup(service => service.GenerateRefreshToken())
                            .Returns($"refresh-{name}");

                        redisServiceMock
                            .Setup(service => service.DeleteCacheWithPatternAsync(It.IsAny<string>()))
                            .Returns(Task.CompletedTask);
                        redisServiceMock
                            .Setup(service => service.SetCacheAsync(
                                It.IsAny<string>(),
                                It.IsAny<object>(),
                                It.IsAny<TimeSpan?>(),
                                true))
                            .ReturnsAsync(true);

                        if (user.Student?.Avatar is not null)
                        {
                            awsStorageServiceMock
                                .Setup(service => service.GetFileUrl(user.Student.Avatar, It.IsAny<CancellationToken>()))
                                .Returns(expectedAvatarUrl!);
                        }
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<UserLoginResponse>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.Equal(user.FirstName, result.Data!.FirstName);
                        Assert.Equal(user.LastName, result.Data.LastName);
                        Assert.Equal(user.UserName, result.Data.Username);
                        Assert.Equal(expectedAvatarUrl, result.Data.AvatarUrl);
                        Assert.Equal($"access-{name}", result.Data.AccessToken);
                        Assert.Equal($"refresh-{name}", result.Data.RefreshToken);
                        Assert.Single(result.Data.Roles);
                        Assert.Equal("Student", result.Data.Roles[0]);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand request,
        Action<UseCaseMockContext> arrangeMocks,
        HttpStatusCode expectedStatusCode,
        string expectedErrorCode)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
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

    private static User CreateUser(bool hasAvatar, string password = "P@ssw0rd!")
    {
        var user = new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FirstName = "Login",
            LastName = "User",
            UserName = "login.user",
            Email = "tester@example.com",
            PasswordHash = HashHelper.HashPassword(password)
        };

        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = Guid.NewGuid(),
            Role = new Role
            {
                Id = Guid.NewGuid(),
                Code = "Student"
            }
        });
        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = Guid.NewGuid(),
            Role = null
        });

        user.Student = new Student
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            UserId = user.Id,
            Avatar = hasAvatar ? "avatars/student.png" : null
        };

        return user;
    }

    private static global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand CreateRequest(LoginByUserCase caseId)
    {
        return caseId switch
        {
            LoginByUserCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand
            {
                Email = "tester@example.com",
                Password = "P@ssw0rd!"
            },
            LoginByUserCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand
            {
                Email = "tester@example.com",
                Password = "P@ssw0rd!"
            },
            LoginByUserCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand
            {
                Email = string.Empty,
                Password = string.Empty
            },
            LoginByUserCase.InvalidUserMissing => new global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand
            {
                Email = "missing@example.com",
                Password = "P@ssw0rd!"
            },
            LoginByUserCase.InvalidPassword => new global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand
            {
                Email = "tester@example.com",
                Password = "P@ssw0rd!"
            },
            LoginByUserCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.LoginByUser.LoginByUserCommand
            {
                Email = "tester@example.com",
                Password = "P@ssw0rd!"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
