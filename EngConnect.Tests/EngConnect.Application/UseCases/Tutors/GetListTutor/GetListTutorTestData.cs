using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetListTutor;

internal enum GetListTutorCase
{
    ValidDefault,
    BoundaryDefault,
    ValidEmptyStatus,
    ValidEmptyVerifiedStatus,
    ExceptionPath,
}

internal static class GetListTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetListTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetListTutor/GetListTutorQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetListTutor/GetListTutorQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetListTutorCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetListTutorCase.BoundaryDefault),
        BuildCase(GetListTutorCase.ValidEmptyStatus),
        BuildCase(GetListTutorCase.ValidEmptyVerifiedStatus),
        BuildCase(GetListTutorCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetListTutorCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } = [];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetListTutorCase.ExceptionPath)
    ];

    public static IEnumerable<object[]> NormalHandlerCases()
    {
        return NormalCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> NormalValidatorCases()
    {
        return Array.Empty<object[]>();
    }

    public static IEnumerable<object[]> HandlerBranchCases()
    {
        return BranchCases
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public static IEnumerable<object[]> ValidatorBranchCases()
    {
        return Array.Empty<object[]>();
    }

    private static UseCaseCaseSet BuildCase(GetListTutorCase caseId)
    {
        return caseId switch
        {
            GetListTutorCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                CreateTutorSeeds()),
            GetListTutorCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId),
                CreateTutorSeeds()),
            GetListTutorCase.ValidEmptyStatus => CreateSuccessCase(
                "valid-empty-status",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                CreateTutorSeeds()),
            GetListTutorCase.ValidEmptyVerifiedStatus => CreateSuccessCase(
                "valid-empty-verified-status",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId),
                CreateTutorSeeds()),
            GetListTutorCase.ExceptionPath => CreateFailureCase(
                "exception-findall-throws",
                CreateRequest(caseId),
                mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                    mocks.StrictMock<IAwsStorageService>();

                    unitOfWorkMock
                        .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                        .Returns(tutorRepositoryMock.Object);

                    tutorRepositoryMock
                        .Setup(repository => repository.FindAll(
                            It.IsAny<Expression<Func<Tutor, bool>>>(),
                            It.IsAny<bool>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<Tutor, object>>[]>()))
                        .Throws(new InvalidOperationException("find all failed"));
                }),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery request,
        IReadOnlyList<Tutor> tutors)
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
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);
                        var awsStorageServiceMock = mocks.StrictMock<IAwsStorageService>();

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                            .Returns(tutorRepositoryMock.Object);

                        tutorRepositoryMock
                            .Setup(repository => repository.FindAll(
                                It.IsAny<Expression<Func<Tutor, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<Tutor, object>>[]>()))
                            .Returns(new TestAsyncEnumerable<Tutor>(tutors));

                        awsStorageServiceMock
                            .Setup(service => service.GetFileUrl("avatars/tutor-1.png", It.IsAny<CancellationToken>()))
                            .Returns("https://files.example/avatars/tutor-1.png");
                        awsStorageServiceMock
                            .Setup(service => service.GetFileUrl("videos/tutor-1.mp4", It.IsAny<CancellationToken>()))
                            .Returns("https://files.example/videos/tutor-1.mp4");
                        awsStorageServiceMock
                            .Setup(service => service.GetFileUrl("docs/tutor-1.pdf", It.IsAny<CancellationToken>()))
                            .Returns("https://files.example/docs/tutor-1.pdf");
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<PaginationResult<GetTutorResponse>>>(resultObject);
                        var items = result.Data!.Items.ToList();

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.True(items.Count >= 1);

                        var firstTutor = items.First();
                        Assert.Equal("https://files.example/avatars/tutor-1.png", firstTutor.Avatar);
                        Assert.Equal("https://files.example/videos/tutor-1.mp4", firstTutor.IntroVideoUrl);
                        Assert.Equal("https://files.example/docs/tutor-1.pdf", firstTutor.CvUrl);

                        if (items.Count > 1)
                        {
                            var secondTutor = items[1];
                            Assert.Null(secondTutor.Avatar);
                            Assert.Null(secondTutor.IntroVideoUrl);
                            Assert.Null(secondTutor.CvUrl);
                        }

                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery request,
        Action<UseCaseMockContext> arrangeMocks)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = UseCaseCaseKind.Exception,
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
                        var result = Assert.IsType<Result<PaginationResult<GetTutorResponse>>>(resultObject);

                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatusCode);
                        Assert.Equal("Server.Error", result.Error?.Code);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static List<Tutor> CreateTutorSeeds()
    {
        return
        [
            new Tutor
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Headline = "IELTS Tutor",
                Bio = "Focus on speaking",
                Status = "Active",
                VerifiedStatus = "Verified",
                Avatar = "avatars/tutor-1.png",
                IntroVideoUrl = "videos/tutor-1.mp4",
                CvUrl = "docs/tutor-1.pdf"
            },
            new Tutor
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Headline = "Boundary Tutor",
                Bio = "Focus on grammar",
                Status = "Active",
                VerifiedStatus = "Verified",
                Avatar = null,
                IntroVideoUrl = null,
                CvUrl = null
            }
        ];
    }

    private static global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery CreateRequest(GetListTutorCase caseId)
    {
        return caseId switch
        {
            GetListTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery("Active", "Verified")
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = string.Empty,
                SortParams = string.Empty
            },
            GetListTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery(string.Empty, string.Empty)
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = string.Empty,
                SortParams = string.Empty
            },
            GetListTutorCase.ValidEmptyStatus => new global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery(string.Empty, "Verified")
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = string.Empty,
                SortParams = string.Empty
            },
            GetListTutorCase.ValidEmptyVerifiedStatus => new global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery("Active", string.Empty)
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = string.Empty,
                SortParams = string.Empty
            },
            GetListTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.GetListTutor.GetListTutorQuery("Active", "Verified")
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = string.Empty,
                SortParams = string.Empty
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
