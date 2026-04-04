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

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetTutorById;

internal enum GetTutorByIdCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class GetTutorByIdTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "GetTutorById",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQuery",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQueryHandler",
        ValidatorTypeFullName = null,
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetTutorById/GetTutorByIdQuery.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/GetTutorById/GetTutorByIdQueryHandler.cs",
        ValidatorSourceRelativePath = null
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(GetTutorByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(GetTutorByIdCase.BoundaryDefault),
        BuildCase(GetTutorByIdCase.InvalidNotFoundOrNull),
        BuildCase(GetTutorByIdCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(GetTutorByIdCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(GetTutorByIdCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(GetTutorByIdCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(GetTutorByIdCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(GetTutorByIdCase caseId)
    {
        return caseId switch
        {
            GetTutorByIdCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            GetTutorByIdCase.BoundaryDefault => CreateSuccessCase("boundary-default", CreateRequest(caseId), introVideoUrl: null, cvUrl: null, avatar: null),
            GetTutorByIdCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            GetTutorByIdCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        global::EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQuery request,
        string? introVideoUrl,
        string? cvUrl,
        string? avatar)
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
                        var tutorRepositoryMock = new Mock<IGenericRepository<Tutor, Guid>>(MockBehavior.Strict);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Tutor, Guid>())
                            .Returns(tutorRepositoryMock.Object);

                        tutorRepositoryMock
                            .Setup(repository => repository.FindAll(
                                It.IsAny<System.Linq.Expressions.Expression<Func<Tutor, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Tutor, object>>[]>()))
                            .Returns(new TestAsyncEnumerable<Tutor>([
                                new Tutor
                                {
                                    Id = request.Id,
                                    UserId = Guid.Parse("61111111-1111-1111-1111-111111111111"),
                                    Headline = "Boundary tutor",
                                    Bio = "Boundary bio",
                                    IntroVideoUrl = introVideoUrl,
                                    CvUrl = cvUrl,
                                    Avatar = avatar,
                                    MonthExperience = 24,
                                    SlotsCount = 2,
                                    Status = "Active",
                                    VerifiedStatus = "Approved",
                                    RatingAverage = 4.5m,
                                    RatingCount = 10,
                                    User = new User
                                    {
                                        FirstName = "Boundary",
                                        LastName = "Tutor",
                                        UserName = "boundary.tutor",
                                        Email = "boundary.tutor@example.com",
                                        Phone = "+84000000001"
                                    }
                                }
                            ]));

                        if (introVideoUrl is not null)
                        {
                            awsStorageMock
                                .Setup(service => service.GetFileUrl(introVideoUrl, It.IsAny<CancellationToken>()))
                                .Returns("https://files.example/intro.mp4");
                        }

                        if (cvUrl is not null)
                        {
                            awsStorageMock
                                .Setup(service => service.GetFileUrl(cvUrl, It.IsAny<CancellationToken>()))
                                .Returns("https://files.example/cv.pdf");
                        }

                        if (avatar is not null)
                        {
                            awsStorageMock
                                .Setup(service => service.GetFileUrl(avatar, It.IsAny<CancellationToken>()))
                                .Returns("https://files.example/avatar.png");
                        }
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<global::EngConnect.Application.UseCases.Tutors.Common.GetTutorResponse>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.Null(result.Data!.IntroVideoUrl);
                        Assert.Null(result.Data.CvUrl);
                        Assert.Null(result.Data.Avatar);
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

    private static global::EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQuery CreateRequest(GetTutorByIdCase caseId)
    {
        return caseId switch
        {
            GetTutorByIdCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")) { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetTutorByIdCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")) { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetTutorByIdCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")) { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            GetTutorByIdCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.GetTutorById.GetTutorByIdQuery(Guid.Parse("11111111-1111-1111-1111-111111111111")) { PageNumber = 1, PageSize = 10, SearchTerm = "SampleValue", SortParams = "createdat-desc" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
