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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.CreateStudent;

internal enum CreateStudentCase
{
    ValidDefault,
    BoundaryNotes200,
    InvalidRequestShape,
    InvalidBoundaryNotes201,
    InvalidUserExistsMissing,
    InvalidDuplicateOrExisting,
    ExceptionPath,
}

internal static class CreateStudentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateStudent",
        RequestTypeFullName = "EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Students/CreateStudent/CreateStudentCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Students/CreateStudent/CreateStudentCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Students/CreateStudent/CreateStudentCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateStudentCase.BoundaryNotes200),
        BuildCase(CreateStudentCase.InvalidRequestShape),
        BuildCase(CreateStudentCase.InvalidBoundaryNotes201),
        BuildCase(CreateStudentCase.InvalidUserExistsMissing),
        BuildCase(CreateStudentCase.InvalidDuplicateOrExisting),
        BuildCase(CreateStudentCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateStudentCase.BoundaryNotes200),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateStudentCase.InvalidRequestShape),
        BuildCase(CreateStudentCase.InvalidBoundaryNotes201),
        BuildCase(CreateStudentCase.InvalidUserExistsMissing),
        BuildCase(CreateStudentCase.InvalidDuplicateOrExisting),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateStudentCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateStudentCase caseId)
    {
        return caseId switch
        {
            CreateStudentCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateStudentCase.BoundaryNotes200 => CreateCase("boundary-Notes-200", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateStudentCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateStudentCase.InvalidBoundaryNotes201 => CreateCase("invalid-boundary-Notes-201", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateStudentCase.InvalidUserExistsMissing => CreateCase("invalid-userExists-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateStudentCase.InvalidDuplicateOrExisting => CreateDuplicateCase(CreateRequest(caseId)),
            CreateStudentCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateDuplicateCase(
        global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand request)
    {
        return new UseCaseCaseSet
        {
            Name = "invalid-duplicate-or-existing",
            Kind = UseCaseCaseKind.Invalid,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = "invalid-duplicate-or-existing",
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                        var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                            .Returns(userRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
                            .Returns(studentRepositoryMock.Object);

                        userRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(true);

                        studentRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<System.Linq.Expressions.Expression<Func<Student, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Student, object>>[]>()))
                            .ReturnsAsync(true);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("StudentId.AlreadyExists", result.Error?.Code);
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

    private static global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand CreateRequest(CreateStudentCase caseId)
    {
        return caseId switch
        {
            CreateStudentCase.ValidDefault => new global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Notes = "Sample description", School = "Sample High School", Grade = "Grade 10", Class = "Class A" },
            CreateStudentCase.BoundaryNotes200 => new global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Notes = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", School = "Sample High School", Grade = "Grade 10", Class = "Class A" },
            CreateStudentCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand { UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Notes = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", School = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Grade = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Class = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" },
            CreateStudentCase.InvalidBoundaryNotes201 => new global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Notes = "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", School = "Sample High School", Grade = "Grade 10", Class = "Class A" },
            CreateStudentCase.InvalidUserExistsMissing => new global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand { UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Notes = "Sample description", School = "Sample High School", Grade = "Grade 10", Class = "Class A" },
            CreateStudentCase.InvalidDuplicateOrExisting => new global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Notes = "Sample description", School = "Sample High School", Grade = "Grade 10", Class = "Class A" },
            CreateStudentCase.ExceptionPath => new global::EngConnect.Application.UseCases.Students.CreateStudent.CreateStudentCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Notes = "Sample description", School = "Sample High School", Grade = "Grade 10", Class = "Class A" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
