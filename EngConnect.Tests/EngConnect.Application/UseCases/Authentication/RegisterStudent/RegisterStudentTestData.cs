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
using MapsterMapper;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterStudent;

internal enum RegisterStudentCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidUserExistsMissing,
    InvalidStudentExists,
    ExceptionPath,
}

internal static class RegisterStudentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "RegisterStudent",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterStudent/RegisterStudentCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterStudent/RegisterStudentCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterStudent/RegisterStudentCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(RegisterStudentCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(RegisterStudentCase.BoundaryDefault),
        BuildCase(RegisterStudentCase.InvalidRequestShape),
        BuildCase(RegisterStudentCase.InvalidUserExistsMissing),
        BuildCase(RegisterStudentCase.InvalidStudentExists),
        BuildCase(RegisterStudentCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(RegisterStudentCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(RegisterStudentCase.InvalidRequestShape),
        BuildCase(RegisterStudentCase.InvalidUserExistsMissing),
        BuildCase(RegisterStudentCase.InvalidStudentExists)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(RegisterStudentCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(RegisterStudentCase caseId)
    {
        return caseId switch
        {
            RegisterStudentCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId)),
            RegisterStudentCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId)),
            RegisterStudentCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            RegisterStudentCase.InvalidUserExistsMissing => CreateFailureCase(
                "invalid-user-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                    mocks.StrictMock<IMapper>();

                    SetupRepositories(unitOfWorkMock, userRepositoryMock, studentRepositoryMock);

                    userRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .ReturnsAsync(false);

                    studentRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<Student, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<Student, object>>[]>()))
                        .ReturnsAsync(false);
                },
                expectedStatusCode: HttpStatusCode.NotFound,
                expectedErrorCode: " User.NotFound"),
            RegisterStudentCase.InvalidStudentExists => CreateFailureCase(
                "invalid-student-exists",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                    mocks.StrictMock<IMapper>();

                    SetupRepositories(unitOfWorkMock, userRepositoryMock, studentRepositoryMock);

                    userRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .ReturnsAsync(true);

                    studentRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<Student, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<Student, object>>[]>()))
                        .ReturnsAsync(true);
                },
                expectedStatusCode: HttpStatusCode.BadRequest,
                expectedErrorCode: "StudentId.AlreadyExists"),
            RegisterStudentCase.ExceptionPath => CreateFailureCase(
                "exception-savechanges-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                    var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                    var mapperMock = mocks.StrictMock<IMapper>();
                    var mappedStudent = new Student
                    {
                        Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                        UserId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                    };

                    SetupRepositories(unitOfWorkMock, userRepositoryMock, studentRepositoryMock);

                    userRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .ReturnsAsync(true);

                    studentRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<Student, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<Student, object>>[]>()))
                        .ReturnsAsync(false);

                    mapperMock
                        .Setup(mapper => mapper.Map<Student>(It.IsAny<global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand>()))
                        .Returns(mappedStudent);

                    studentRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<Student>()));

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                        .ThrowsAsync(new InvalidOperationException("save failed"));
                },
                expectedStatusCode: HttpStatusCode.InternalServerError,
                expectedErrorCode: "Server.Error"),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand request)
    {
        Student? addedStudent = null;
        var mappedStudent = new Student
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            UserId = request.UserId,
            Notes = request.Notes,
            School = request.School,
            Grade = request.Grade,
            Class = request.Class
        };

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
                        var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                        var mapperMock = mocks.StrictMock<IMapper>();

                        SetupRepositories(unitOfWorkMock, userRepositoryMock, studentRepositoryMock);

                        userRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<User, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(true);

                        studentRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<Student, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Student, object>>[]>()))
                            .ReturnsAsync(false);

                        mapperMock
                            .Setup(mapper => mapper.Map<Student>(It.IsAny<global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand>()))
                            .Returns(mappedStudent);

                        studentRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<Student>()))
                            .Callback<Student>(entity => addedStudent = entity);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<Student>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(addedStudent);
                        Assert.Equal(nameof(StudentStatus.Active), addedStudent!.Status);
                        Assert.Equal(request.UserId, addedStudent.UserId);
                        Assert.Equal(request.UserId, result.Data?.UserId);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand request,
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
        Mock<IGenericRepository<Student, Guid>> studentRepositoryMock)
    {
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
            .Returns(userRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
            .Returns(studentRepositoryMock.Object);
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

    private static global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand CreateRequest(RegisterStudentCase caseId)
    {
        return caseId switch
        {
            RegisterStudentCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand
            {
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Notes = "Needs evening classes",
                School = "Nguyen Trai High School",
                Grade = "Grade 10",
                Class = "10A1"
            },
            RegisterStudentCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand
            {
                UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Notes = string.Empty,
                School = "Boundary School",
                Grade = "Grade 1",
                Class = "1A"
            },
            RegisterStudentCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand
            {
                UserId = Guid.Empty,
                Notes = new string('x', 600),
                School = new string('x', 600),
                Grade = new string('x', 600),
                Class = new string('x', 600)
            },
            RegisterStudentCase.InvalidUserExistsMissing => new global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand
            {
                UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Notes = "Missing user",
                School = "Missing User School",
                Grade = "Grade 9",
                Class = "9A1"
            },
            RegisterStudentCase.InvalidStudentExists => new global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand
            {
                UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Notes = "Existing student",
                School = "Existing Student School",
                Grade = "Grade 11",
                Class = "11A2"
            },
            RegisterStudentCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.RegisterStudent.RegisterStudentCommand
            {
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Notes = "Exception branch",
                School = "Save Error School",
                Grade = "Grade 12",
                Class = "12A3"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
