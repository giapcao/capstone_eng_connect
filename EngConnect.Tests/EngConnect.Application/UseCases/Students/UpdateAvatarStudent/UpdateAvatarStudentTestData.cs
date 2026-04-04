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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateAvatarStudent;

internal enum UpdateAvatarStudentCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidStudentExistMissing,
    InvalidUpdateFileRejected,
    ExceptionPath,
}

internal static class UpdateAvatarStudentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "UpdateAvatarStudent",
        RequestTypeFullName = "EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Students/UpdateAvatarStudent/UpdateAvatarStudentCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Students/UpdateAvatarStudent/UpdateAvatarStudentCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Students/UpdateAvatarStudent/UpdateAvatarValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(UpdateAvatarStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(UpdateAvatarStudentCase.BoundaryDefault),
        BuildCase(UpdateAvatarStudentCase.InvalidRequestShape),
        BuildCase(UpdateAvatarStudentCase.InvalidStudentExistMissing),
        BuildCase(UpdateAvatarStudentCase.InvalidUpdateFileRejected),
        BuildCase(UpdateAvatarStudentCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(UpdateAvatarStudentCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(UpdateAvatarStudentCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(UpdateAvatarStudentCase.InvalidRequestShape),
        BuildCase(UpdateAvatarStudentCase.InvalidStudentExistMissing),
        BuildCase(UpdateAvatarStudentCase.InvalidUpdateFileRejected),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(UpdateAvatarStudentCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(UpdateAvatarStudentCase caseId)
    {
        return caseId switch
        {
            UpdateAvatarStudentCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateAvatarStudentCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateAvatarStudentCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            UpdateAvatarStudentCase.InvalidStudentExistMissing => CreateCase("invalid-studentExist-missing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            UpdateAvatarStudentCase.InvalidUpdateFileRejected => CreateFailureCase("invalid-update-file-rejected", CreateRequest(caseId)),
            UpdateAvatarStudentCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand request)
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
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var awsStorageMock = mocks.StrictMock<IAwsStorageService>();
                        var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
                            .Returns(studentRepositoryMock.Object);

                        studentRepositoryMock
                            .Setup(repository => repository.FindByIdAsync(
                                request.Id,
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Student, object>>[]>()))
                            .ReturnsAsync(new Student
                            {
                                Id = request.Id,
                                Avatar = "avatars/original-student.png",
                                Status = nameof(StudentStatus.Active)
                            });

                        awsStorageMock
                            .Setup(service => service.UpdateFileAsync(
                                request.File,
                                request.Id,
                                nameof(PrefixFile.StudentAvatar),
                                It.IsAny<CancellationToken>()))
                            .ReturnsAsync((global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUploadResult?)null);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsAssignableFrom<Result>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
                        Assert.Equal("Validation.Failed", result.Error?.Code);
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

    private static global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand CreateRequest(UpdateAvatarStudentCase caseId)
    {
        return caseId switch
        {
            UpdateAvatarStudentCase.ValidDefault => new global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateAvatarStudentCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateAvatarStudentCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar", ContentType = "image/png", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("00000000-0000-0000-0000-000000000000") },
            UpdateAvatarStudentCase.InvalidStudentExistMissing => new global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
            UpdateAvatarStudentCase.InvalidUpdateFileRejected => new global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            UpdateAvatarStudentCase.ExceptionPath => new global::EngConnect.Application.UseCases.Students.UpdateAvatarStudent.UpdateAvatarStudentCommand { File = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "avatar.png", ContentType = "image/png", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, Id = Guid.Parse("11111111-1111-1111-1111-111111111111") },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
