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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetStudentById;

internal enum GetStudentByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class GetStudentByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetStudentById",
        RequestTypeFullName = "EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Students/GetStudentById/GetStudentByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Students/GetStudentById/GetStudentByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetStudentByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetStudentByIdCase.BoundaryDefault),
        BuildCase(GetStudentByIdCase.InvalidNotFoundOrNull),
        BuildCase(GetStudentByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetStudentByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetStudentByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetStudentByIdCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetStudentByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetStudentByIdCase caseId)
    {
        return caseId switch
        {
            GetStudentByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetStudentByIdCase.BoundaryDefault => CreateSuccessCase("boundary-default", CreateRequest(caseId), avatarPath: null, expectedAvatarUrl: null),
            GetStudentByIdCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            GetStudentByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        global::EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQuery request,
        string? avatarPath,
        string? expectedAvatarUrl)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = UseCaseCaseKind.Boundary,
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
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var awsStorageMock = mocks.StrictMock<IAwsStorageService>();
                        var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
                            .Returns(studentRepositoryMock.Object);

                        studentRepositoryMock
                            .Setup(repository => repository.FindAll(
                                It.IsAny<System.Linq.Expressions.Expression<Func<Student, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Student, object>>[]>()))
                            .Returns(new TestAsyncEnumerable<Student>([
                                new Student
                                {
                                    Id = request.Id,
                                    UserId = Guid.Parse("51111111-1111-1111-1111-111111111111"),
                                    Notes = "Boundary student",
                                    School = "Boundary school",
                                    Grade = "Grade 11",
                                    Class = "Class B",
                                    Avatar = avatarPath,
                                    Status = "Active",
                                    User = new User
                                    {
                                        FirstName = "Boundary",
                                        LastName = "Student",
                                        UserName = "boundary.student",
                                        Email = "boundary.student@example.com",
                                        Phone = "+84000000000"
                                    }
                                }
                            ]));

                        if (avatarPath is not null)
                        {
                            awsStorageMock
                                .Setup(service => service.GetFileUrl(avatarPath, It.IsAny<CancellationToken>()))
                                .Returns(expectedAvatarUrl!);
                        }
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<global::EngConnect.Application.UseCases.Students.Common.GetStudentResponse>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.Equal(expectedAvatarUrl, result.Data!.Avatar);
                        Assert.Equal("Boundary", result.Data.User.FirstName);
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

    private static global::EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQuery CreateRequest(GetStudentByIdCase caseId)
    {
        return caseId switch
        {
            GetStudentByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetStudentByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetStudentByIdCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            GetStudentByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.Students.GetStudentById.GetStudentByIdQuery { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
