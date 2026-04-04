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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor;

internal enum GetIntroVideoUrlTutorCase
{
    ValidCacheHit,
    BoundaryRepositoryHit,
    InvalidTutorMissing,
    InvalidIntroVideoMissing,
    ExceptionCacheThrows,
}

internal static class GetIntroVideoUrlTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetIntroVideoUrlTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor.GetIntroVideoUrlTutorQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor.GetIntroVideoUrlTutorQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetIntroVideoUrlTutor/GetIntroVideoUrlTutorQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetIntroVideoUrlTutor/GetIntroVideoUrlTutorQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly Guid TutorId = Guid.Parse("41111111-1111-1111-1111-111111111111");
    private static readonly string CacheKey = RedisKeyGenerator.GenerateTutorIntroVideoKey(TutorId);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetIntroVideoUrlTutorCase.ValidCacheHit)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetIntroVideoUrlTutorCase.BoundaryRepositoryHit),
        BuildCase(GetIntroVideoUrlTutorCase.InvalidTutorMissing),
        BuildCase(GetIntroVideoUrlTutorCase.InvalidIntroVideoMissing),
        BuildCase(GetIntroVideoUrlTutorCase.ExceptionCacheThrows)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetIntroVideoUrlTutorCase.BoundaryRepositoryHit)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetIntroVideoUrlTutorCase.InvalidTutorMissing),
        BuildCase(GetIntroVideoUrlTutorCase.InvalidIntroVideoMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetIntroVideoUrlTutorCase.ExceptionCacheThrows)
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

    private static UseCaseCaseSet BuildCase(GetIntroVideoUrlTutorCase caseId)
    {
        return caseId switch
        {
            GetIntroVideoUrlTutorCase.ValidCacheHit => CreateSuccessCase(
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
                        .ReturnsAsync("intro/cache-tutor.mp4");
                    awsStorageMock
                        .Setup(service => service.GetFileUrl("intro/cache-tutor.mp4", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/cache-tutor-intro.mp4");
                },
                expectedRelativePath: "intro/cache-tutor.mp4",
                expectedUrl: "https://files.example/cache-tutor-intro.mp4"),
            GetIntroVideoUrlTutorCase.BoundaryRepositoryHit => CreateSuccessCase(
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
                        .Setup(service => service.SetCacheAsync(CacheKey, "intro/repository-tutor.mp4", It.IsAny<TimeSpan?>(), false))
                        .ReturnsAsync(true);

                    tutorRepositoryMock
                        .Setup(repository => repository.FindByIdAsync(TutorId, false, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Tutor, object>>[]>()))
                        .ReturnsAsync(new Tutor
                        {
                            Id = TutorId,
                            IntroVideoUrl = "intro/repository-tutor.mp4"
                        });

                    awsStorageMock
                        .Setup(service => service.GetFileUrl("intro/repository-tutor.mp4", It.IsAny<CancellationToken>()))
                        .Returns("https://files.example/repository-tutor-intro.mp4");
                },
                expectedRelativePath: "intro/repository-tutor.mp4",
                expectedUrl: "https://files.example/repository-tutor-intro.mp4"),
            GetIntroVideoUrlTutorCase.InvalidTutorMissing => CreateFailureCase(
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
            GetIntroVideoUrlTutorCase.InvalidIntroVideoMissing => CreateFailureCase(
                "invalid-intro-video-missing",
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
                            IntroVideoUrl = null
                        });
                },
                expectedStatusCode: HttpStatusCode.NotFound),
            GetIntroVideoUrlTutorCase.ExceptionCacheThrows => CreateFailureCase(
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
        global::EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor.GetIntroVideoUrlTutorQuery request,
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
                        var result = Assert.IsType<Result<global::EngConnect.Application.UseCases.Tutors.Common.GetIntroVideoUrlTutorResponse>>(resultObject);

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
        global::EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor.GetIntroVideoUrlTutorQuery request,
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

    private static global::EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor.GetIntroVideoUrlTutorQuery CreateRequest()
    {
        return new global::EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor.GetIntroVideoUrlTutorQuery
        {
            Id = TutorId
        };
    }
}
