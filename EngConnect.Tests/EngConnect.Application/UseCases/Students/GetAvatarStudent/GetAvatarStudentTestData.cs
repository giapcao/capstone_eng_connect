using System.Linq.Expressions;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetAvatarStudent;

internal enum GetAvatarStudentCase
{
    ValidCacheHit,
    BoundaryRepositoryHit,
    InvalidStudentMissing,
    InvalidAvatarMissing,
    ExceptionCacheThrows,
}

internal static class GetAvatarStudentTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetAvatarStudent",
        RequestTypeFullName = "EngConnect.Application.UseCases.Students.GetAvatarStudent.GetAvatarQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Students.GetAvatarStudent.GetAvatarQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Students/GetAvatarStudent/GetAvatarQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Students/GetAvatarStudent/GetAvatarQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly Guid StudentId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly string CacheKey = RedisKeyGenerator.GenerateStudentAvatarKey(StudentId);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetAvatarStudentCase.ValidCacheHit)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetAvatarStudentCase.BoundaryRepositoryHit),
        BuildCase(GetAvatarStudentCase.InvalidStudentMissing),
        BuildCase(GetAvatarStudentCase.InvalidAvatarMissing),
        BuildCase(GetAvatarStudentCase.ExceptionCacheThrows)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetAvatarStudentCase.BoundaryRepositoryHit)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetAvatarStudentCase.InvalidStudentMissing),
        BuildCase(GetAvatarStudentCase.InvalidAvatarMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetAvatarStudentCase.ExceptionCacheThrows)
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

    private static UseCaseCaseSet BuildCase(GetAvatarStudentCase caseId)
    {
        return caseId switch
        {
            GetAvatarStudentCase.ValidCacheHit => CreateSuccessCase(
                "valid-cache-hit",
                UseCaseCaseKind.Valid,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    var awsStorageMock = mocks.StrictMock<IAwsStorageService>();
                    OverrideSettings(mocks, 15);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ReturnsAsync("avatars/cache-student.png");
                    awsStorageMock
                        .Setup(service => service.GetFileUrl("avatars/cache-student.png", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/cache-student.png");
                },
                expectedRelativePath: "avatars/cache-student.png",
                expectedUrl: "https://files.example/cache-student.png"),
            GetAvatarStudentCase.BoundaryRepositoryHit => CreateSuccessCase(
                "boundary-repository-hit",
                UseCaseCaseKind.Boundary,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    var awsStorageMock = mocks.StrictMock<IAwsStorageService>();
                    var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                    OverrideSettings(mocks, 1);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
                        .Returns(studentRepositoryMock.Object);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ReturnsAsync((string?)null);
                    cacheMock
                        .Setup(service => service.SetCacheAsync(CacheKey, "avatars/repository-student.png", It.IsAny<TimeSpan?>(), false))
                        .ReturnsAsync(true);

                    studentRepositoryMock
                        .Setup(repository => repository.FindByIdAsync(StudentId, false, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Student, object>>[]>()))
                        .ReturnsAsync(new Student
                        {
                            Id = StudentId,
                            Avatar = "avatars/repository-student.png"
                        });

                    awsStorageMock
                        .Setup(service => service.GetFileUrl("avatars/repository-student.png", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/repository-student.png");
                },
                expectedRelativePath: "avatars/repository-student.png",
                expectedUrl: "https://files.example/repository-student.png"),
            GetAvatarStudentCase.InvalidStudentMissing => CreateFailureCase(
                "invalid-student-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                    OverrideSettings(mocks, 15);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
                        .Returns(studentRepositoryMock.Object);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ReturnsAsync((string?)null);

                    studentRepositoryMock
                        .Setup(repository => repository.FindByIdAsync(StudentId, false, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Student, object>>[]>()))
                        .ReturnsAsync((Student?)null);
                },
                expectedStatusCode: HttpStatusCode.NotFound),
            GetAvatarStudentCase.InvalidAvatarMissing => CreateFailureCase(
                "invalid-avatar-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    var studentRepositoryMock = new Mock<IGenericRepository<Student, Guid>>(MockBehavior.Strict);
                    OverrideSettings(mocks, 15);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Student, Guid>())
                        .Returns(studentRepositoryMock.Object);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ReturnsAsync((string?)null);

                    studentRepositoryMock
                        .Setup(repository => repository.FindByIdAsync(StudentId, false, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Student, object>>[]>()))
                        .ReturnsAsync(new Student
                        {
                            Id = StudentId,
                            Avatar = null
                        });
                },
                expectedStatusCode: HttpStatusCode.NotFound),
            GetAvatarStudentCase.ExceptionCacheThrows => CreateFailureCase(
                "exception-cache-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    OverrideSettings(mocks, 15);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ThrowsAsync(new InvalidOperationException("redis unavailable"));
                },
                expectedStatusCode: HttpStatusCode.InternalServerError),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Students.GetAvatarStudent.GetAvatarQuery request,
        Action<UseCaseMockContext> arrangeMocks,
        string expectedRelativePath,
        string expectedUrl)
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
                    ArrangeMocks = arrangeMocks,
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<global::EngConnect.Application.UseCases.Students.Common.GetAvatarResponse>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.Equal(expectedRelativePath, result.Data!.RelativePath);
                        Assert.Equal(expectedUrl, result.Data.Url);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Students.GetAvatarStudent.GetAvatarQuery request,
        Action<UseCaseMockContext> arrangeMocks,
        HttpStatusCode expectedStatusCode)
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
                        var result = Assert.IsAssignableFrom<Result>(resultObject);

                        Assert.True(result.IsFailure);
                        Assert.Equal(expectedStatusCode, result.HttpStatusCode);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static void OverrideSettings(UseCaseMockContext mocks, int expirationMinutes)
    {
        mocks.Override<IOptions<RedisCacheSettings>>(Options.Create(new RedisCacheSettings
        {
            SettingCacheExpirationMinutes = expirationMinutes
        }));
    }

    private static global::EngConnect.Application.UseCases.Students.GetAvatarStudent.GetAvatarQuery CreateRequest()
    {
        return new global::EngConnect.Application.UseCases.Students.GetAvatarStudent.GetAvatarQuery
        {
            Id = StudentId
        };
    }
}
