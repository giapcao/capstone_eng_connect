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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetAvatarTutor;

internal enum GetAvatarTutorCase
{
    ValidCacheHit,
    BoundaryRepositoryHit,
    InvalidTutorMissing,
    InvalidAvatarMissing,
    ExceptionCacheThrows,
}

internal static class GetAvatarTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetAvatarTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.GetAvatarTutor.GetAvatarTutorQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.GetAvatarTutor.GetAvatarTutorQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetAvatarTutor/GetAvatarTutorQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetAvatarTutor/GetAvatarTutorQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly Guid TutorId = Guid.Parse("21111111-1111-1111-1111-111111111111");
    private static readonly string CacheKey = RedisKeyGenerator.GenerateTutorAvatarKey(TutorId);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetAvatarTutorCase.ValidCacheHit)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetAvatarTutorCase.BoundaryRepositoryHit),
        BuildCase(GetAvatarTutorCase.InvalidTutorMissing),
        BuildCase(GetAvatarTutorCase.InvalidAvatarMissing),
        BuildCase(GetAvatarTutorCase.ExceptionCacheThrows)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetAvatarTutorCase.BoundaryRepositoryHit)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetAvatarTutorCase.InvalidTutorMissing),
        BuildCase(GetAvatarTutorCase.InvalidAvatarMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetAvatarTutorCase.ExceptionCacheThrows)
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

    private static UseCaseCaseSet BuildCase(GetAvatarTutorCase caseId)
    {
        return caseId switch
        {
            GetAvatarTutorCase.ValidCacheHit => CreateSuccessCase(
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
                        .ReturnsAsync("avatars/cache-tutor.png");
                    awsStorageMock
                        .Setup(service => service.GetFileUrl("avatars/cache-tutor.png", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/cache-tutor.png");
                    awsStorageMock
                        .Setup(service => service.GetFileUrl("avatars/cache-tutor.png", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/cache-tutor.png");
                },
                expectedRelativePath: "avatars/cache-tutor.png",
                expectedUrl: "https://files.example/cache-tutor.png"),
            GetAvatarTutorCase.BoundaryRepositoryHit => CreateSuccessCase(
                "boundary-repository-hit",
                UseCaseCaseKind.Boundary,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    var awsStorageMock = mocks.StrictMock<IAwsStorageService>();
                    var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                    OverrideSettings(mocks, 1);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                        .Returns(tutorRepositoryMock.Object);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ReturnsAsync((string?)null);
                    cacheMock
                        .Setup(service => service.SetCacheAsync(CacheKey, "avatars/repository-tutor.png", It.IsAny<TimeSpan?>(), false))
                        .ReturnsAsync(true);

                    tutorRepositoryMock
                        .Setup(repository => repository.FindByIdAsync(TutorId, false, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Tutor, object>>[]>()))
                        .ReturnsAsync(new Tutor
                        {
                            Id = TutorId,
                            Avatar = "avatars/repository-tutor.png"
                        });

                    awsStorageMock
                        .Setup(service => service.GetFileUrl("avatars/repository-tutor.png", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/repository-tutor.png");
                },
                expectedRelativePath: "avatars/repository-tutor.png",
                expectedUrl: "https://files.example/repository-tutor.png"),
            GetAvatarTutorCase.InvalidTutorMissing => CreateFailureCase(
                "invalid-tutor-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                    OverrideSettings(mocks, 15);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                        .Returns(tutorRepositoryMock.Object);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ReturnsAsync((string?)null);

                    tutorRepositoryMock
                        .Setup(repository => repository.FindByIdAsync(TutorId, false, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Tutor, object>>[]>()))
                        .ReturnsAsync((Tutor?)null);
                },
                expectedStatusCode: HttpStatusCode.NotFound),
            GetAvatarTutorCase.InvalidAvatarMissing => CreateFailureCase(
                "invalid-avatar-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var cacheMock = mocks.StrictMock<IRedisService>();
                    var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                    OverrideSettings(mocks, 15);

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                        .Returns(tutorRepositoryMock.Object);

                    cacheMock
                        .Setup(service => service.GetCacheAsync(CacheKey))
                        .ReturnsAsync((string?)null);

                    tutorRepositoryMock
                        .Setup(repository => repository.FindByIdAsync(TutorId, false, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Tutor, object>>[]>()))
                        .ReturnsAsync(new Tutor
                        {
                            Id = TutorId,
                            Avatar = null
                        });
                },
                expectedStatusCode: HttpStatusCode.NotFound),
            GetAvatarTutorCase.ExceptionCacheThrows => CreateFailureCase(
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
        global::EngConnect.Application.UseCases.Tutors.GetAvatarTutor.GetAvatarTutorQuery request,
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
                        var result = Assert.IsType<Result<global::EngConnect.Application.UseCases.Tutors.Common.GetAvatarTutorResponse>>(resultObject);

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
        global::EngConnect.Application.UseCases.Tutors.GetAvatarTutor.GetAvatarTutorQuery request,
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

    private static global::EngConnect.Application.UseCases.Tutors.GetAvatarTutor.GetAvatarTutorQuery CreateRequest()
    {
        return new global::EngConnect.Application.UseCases.Tutors.GetAvatarTutor.GetAvatarTutorQuery
        {
            Id = TutorId
        };
    }
}
