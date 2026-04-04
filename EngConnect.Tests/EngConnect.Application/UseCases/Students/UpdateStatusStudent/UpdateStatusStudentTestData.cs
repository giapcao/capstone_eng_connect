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
using EngConnect.Domain.Constants;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateStatusStudent;

internal enum UpdateStatusStudentCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidStudentExistMissing,
    ExceptionPath,
}

internal static class UpdateStatusStudentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateStatusStudent",
        RequestTypeFullName = "EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommandHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Students/UpdateStatusStudent/UpdateStatusStudentCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Students/UpdateStatusStudent/UpdateStatusStudentCommandHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateStatusStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateStatusStudentCase.BoundaryDefault),
        BuildCase(UpdateStatusStudentCase.InvalidStudentExistMissing),
        BuildCase(UpdateStatusStudentCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateStatusStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateStatusStudentCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateStatusStudentCase.InvalidStudentExistMissing),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateStatusStudentCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateStatusStudentCase caseId)
    {
        return caseId switch
        {
            UpdateStatusStudentCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            UpdateStatusStudentCase.BoundaryDefault => CreateSuccessCase("boundary-default", CreateRequest(caseId), nameof(StudentStatus.Inactive), nameof(StudentStatus.Active)),
            UpdateStatusStudentCase.InvalidStudentExistMissing => CreateCase("invalid-studentExist-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            UpdateStatusStudentCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        global::EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommand request,
        string initialStatus,
        string expectedStatus)
    {
        Student? updatedStudent = null;

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
                        var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
                            .Returns(studentRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);

                        studentRepositoryMock
                            .Setup(repository => repository.FindByIdAsync(
                                request.Id,
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Student, object>>[]>()))
                            .ReturnsAsync(new Student
                            {
                                Id = request.Id,
                                Status = initialStatus
                            });

                        studentRepositoryMock
                            .Setup(repository => repository.Update(It.IsAny<Student>()))
                            .Callback<Student>(entity => updatedStudent = entity);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(updatedStudent);
                        Assert.Equal(expectedStatus, updatedStudent!.Status);
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

    private static global::EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommand CreateRequest(UpdateStatusStudentCase caseId)
    {
        return caseId switch
        {
            UpdateStatusStudentCase.ValidDefault => new global::EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateStatusStudentCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateStatusStudentCase.InvalidStudentExistMissing => new global::EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommand { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            UpdateStatusStudentCase.ExceptionPath => new global::EngConnect.Application.UseCases.Students.UpdateStatusStudent.UpdateStatusStudentCommand { Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
