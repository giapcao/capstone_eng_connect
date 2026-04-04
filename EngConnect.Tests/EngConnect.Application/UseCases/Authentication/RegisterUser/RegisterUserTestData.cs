using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUser;

internal enum RegisterUserCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidIsEmailExistExisting,
    ExceptionPath,
}

internal static class RegisterUserTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "RegisterUser",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterUser/RegisterUserCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterUser/RegisterUserCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterUser/RegisterUserCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(RegisterUserCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(RegisterUserCase.BoundaryDefault),
        BuildCase(RegisterUserCase.InvalidRequestShape),
        BuildCase(RegisterUserCase.InvalidIsEmailExistExisting),
        BuildCase(RegisterUserCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(RegisterUserCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(RegisterUserCase.InvalidRequestShape),
        BuildCase(RegisterUserCase.InvalidIsEmailExistExisting)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(RegisterUserCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(RegisterUserCase caseId)
    {
        return caseId switch
        {
            RegisterUserCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                roleExists: true,
                expectStudentNotes: true),
            RegisterUserCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                roleExists: false,
                expectStudentNotes: false),
            RegisterUserCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            RegisterUserCase.InvalidIsEmailExistExisting => CreateFailureCase(
                "invalid-email-existing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                        .Returns(userRepositoryMock.Object);

                    userRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .ReturnsAsync(true);
                },
                expectedStatusCode: HttpStatusCode.BadRequest,
                expectedErrorCode: "User.AlreadyExists"),
            RegisterUserCase.ExceptionPath => CreateFailureCase(
                "exception-redis-set-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var redisServiceMock = mocks.StrictMock<IRedisService>();
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                    var roleRepositoryMock = new Mock<IGenericRepository<Role, Guid>>(MockBehavior.Strict);
                    var userRoleRepositoryMock = new Mock<IGenericRepository<UserRole, Guid>>(MockBehavior.Strict);
                    var outboxRepositoryMock = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);
                    var transactionMock = CreateTransactionMock(Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"));

                    SetupRepositories(unitOfWorkMock, userRepositoryMock, studentRepositoryMock, roleRepositoryMock, userRoleRepositoryMock, outboxRepositoryMock);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                        .ReturnsAsync(transactionMock.Object);
                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.RollbackTransactionAsync())
                        .Returns(Task.CompletedTask);

                    userRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .ReturnsAsync(false);
                    userRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<User>()));

                    studentRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<Student>()));

                    roleRepositoryMock
                        .Setup(repository => repository.FindFirstAsync(
                            It.IsAny<Expression<Func<Role, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<Role, object>>[]>()))
                        .ReturnsAsync(new Role
                        {
                            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            Code = "Student"
                        });

                    userRoleRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<UserRole>()));

                    outboxRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<OutboxEvent>()));

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                        .ReturnsAsync(1);

                    redisServiceMock
                        .Setup(service => service.SetCacheAsync(
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<TimeSpan?>(),
                            true))
                        .ThrowsAsync(new InvalidOperationException("redis set failed"));
                },
                expectedStatusCode: HttpStatusCode.InternalServerError,
                expectedErrorCode: "Server.Error"),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand request,
        bool roleExists,
        bool expectStudentNotes)
    {
        User? addedUser = null;
        Student? addedStudent = null;
        UserRole? addedUserRole = null;
        OutboxEvent? addedOutboxEvent = null;
        string? redisValue = null;

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
                        var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                        var roleRepositoryMock = new Mock<IGenericRepository<Role, Guid>>(MockBehavior.Strict);
                        var userRoleRepositoryMock = new Mock<IGenericRepository<UserRole, Guid>>(MockBehavior.Strict);
                        var outboxRepositoryMock = new Mock<IGenericRepository<OutboxEvent, Guid>>(MockBehavior.Strict);
                        var transactionMock = CreateTransactionMock(Guid.Parse("aaaaaaaa-1111-2222-3333-aaaaaaaaaaaa"));

                        SetupRepositories(unitOfWorkMock, userRepositoryMock, studentRepositoryMock, roleRepositoryMock, userRoleRepositoryMock, outboxRepositoryMock);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                            .ReturnsAsync(transactionMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.CommitTransactionAsync())
                            .Returns(Task.CompletedTask);

                        userRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<User, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(false);
                        userRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<User>()))
                            .Callback<User>(entity => addedUser = entity);

                        studentRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<Student>()))
                            .Callback<Student>(entity => addedStudent = entity);

                        roleRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<Expression<Func<Role, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Role, object>>[]>()))
                            .ReturnsAsync(roleExists
                                ? new Role
                                {
                                    Id = Guid.Parse("bbbbbbbb-2222-3333-4444-bbbbbbbbbbbb"),
                                    Code = "Student"
                                }
                                : null);

                        userRoleRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<UserRole>()))
                            .Callback<UserRole>(entity => addedUserRole = entity);

                        outboxRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<OutboxEvent>()))
                            .Callback<OutboxEvent>(entity => addedOutboxEvent = entity);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);

                        redisServiceMock
                            .Setup(service => service.SetCacheAsync(
                                It.IsAny<string>(),
                                It.IsAny<object>(),
                                It.IsAny<TimeSpan?>(),
                                true))
                            .Callback<string, object, TimeSpan?, bool>((_, value, _, _) => redisValue = value.ToString())
                            .ReturnsAsync(true);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(addedUser);
                        Assert.NotNull(addedStudent);
                        Assert.NotNull(addedOutboxEvent);
                        Assert.Equal(addedUser!.Id, addedStudent!.UserId);
                        Assert.Equal(expectStudentNotes ? request.Notes : null, addedStudent.Notes);
                        Assert.Equal(addedUser.Id.ToString(), redisValue);

                        if (roleExists)
                        {
                            Assert.NotNull(addedUserRole);
                            Assert.Equal(addedUser.Id, addedUserRole!.UserId);
                        }
                        else
                        {
                            Assert.Null(addedUserRole);
                        }

                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand request,
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

    private static void SetupRepositories(
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IGenericRepository<User, Guid>> userRepositoryMock,
        Mock<IGenericRepository<Student, Guid>> studentRepositoryMock,
        Mock<IGenericRepository<Role, Guid>> roleRepositoryMock,
        Mock<IGenericRepository<UserRole, Guid>> userRoleRepositoryMock,
        Mock<IGenericRepository<OutboxEvent, Guid>> outboxRepositoryMock)
    {
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
            .Returns(userRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
            .Returns(studentRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Role, Guid>())
            .Returns(roleRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<UserRole, Guid>())
            .Returns(userRoleRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<OutboxEvent, Guid>())
            .Returns(outboxRepositoryMock.Object);
    }

    private static Mock<IDbContextTransaction> CreateTransactionMock(Guid transactionId)
    {
        var transactionMock = new Mock<IDbContextTransaction>(MockBehavior.Strict);
        transactionMock.SetupGet(transaction => transaction.TransactionId).Returns(transactionId);
        return transactionMock;
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

    private static global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand CreateRequest(RegisterUserCase caseId)
    {
        return caseId switch
        {
            RegisterUserCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand
            {
                FirstName = "Sample",
                LastName = "Tester",
                Email = "registerusercommand@example.com",
                UserName = "sample.user",
                Phone = "+84987654321",
                AddressNum = "12 Nguyen Hue",
                ProvinceId = "79",
                ProvinceName = "Ho Chi Minh",
                WardId = "26734",
                WardName = "Ben Nghe",
                Password = "P@ssw0rd!",
                Notes = "Parent prefers evening classes",
                School = "Tran Dai Nghia High School",
                Grade = "Grade 10",
                Class = "10A1"
            },
            RegisterUserCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand
            {
                FirstName = "A",
                LastName = "B",
                Email = "boundary.user@example.com",
                UserName = "boundary.user",
                Phone = "+84123456789",
                AddressNum = "1",
                ProvinceId = "01",
                ProvinceName = "Ha Noi",
                WardId = "00001",
                WardName = "Ward 1",
                Password = "P@ssw0rd!",
                Notes = string.Empty,
                School = "Boundary School",
                Grade = "Grade 1",
                Class = "1A"
            },
            RegisterUserCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand
            {
                FirstName = new string('x', 80),
                LastName = new string('x', 80),
                Email = string.Empty,
                UserName = new string('x', 60),
                Phone = "invalid-phone",
                AddressNum = new string('x', 120),
                ProvinceId = string.Empty,
                ProvinceName = "X",
                WardId = string.Empty,
                WardName = "Y",
                Password = string.Empty,
                Notes = "Invalid request",
                School = "Invalid School",
                Grade = "Invalid Grade",
                Class = "Invalid Class"
            },
            RegisterUserCase.InvalidIsEmailExistExisting => new global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "tester@example.com",
                UserName = "existing.user",
                Phone = "+84987654321",
                AddressNum = "34 Le Loi",
                ProvinceId = "79",
                ProvinceName = "Ho Chi Minh",
                WardId = "26734",
                WardName = "Ben Nghe",
                Password = "P@ssw0rd!",
                Notes = "Duplicate email",
                School = "Duplicate School",
                Grade = "Grade 11",
                Class = "11A2"
            },
            RegisterUserCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.RegisterUser.RegisterUserCommand
            {
                FirstName = "Exception",
                LastName = "User",
                Email = "exception.user@example.com",
                UserName = "exception.user",
                Phone = "+84987650000",
                AddressNum = "56 Tran Hung Dao",
                ProvinceId = "48",
                ProvinceName = "Da Nang",
                WardId = "20194",
                WardName = "Hai Chau",
                Password = "P@ssw0rd!",
                Notes = "Exception branch",
                School = "Exception School",
                Grade = "Grade 12",
                Class = "12A3"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
