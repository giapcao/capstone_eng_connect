using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Tests.Common;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;

internal enum CreateSupportTicketMessageCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidDuplicateOrExisting,
    InvalidTicketMissing,
    InvalidSenderMissing,
    ExceptionPath,
}

internal static class CreateSupportTicketMessageTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateSupportTicketMessage",
        RequestTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/CreateSupportTicketMessage/CreateSupportTicketMessageCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/CreateSupportTicketMessage/CreateSupportTicketMessageCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/SupportTicketMessages/CreateSupportTicketMessage/CreateSupportTicketMessageCommandValidator.cs"
    };

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateSupportTicketMessageCase.ValidDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateSupportTicketMessageCase.BoundaryDefault),
        BuildCase(CreateSupportTicketMessageCase.InvalidRequestShape),
        BuildCase(CreateSupportTicketMessageCase.InvalidDuplicateOrExisting),
        BuildCase(CreateSupportTicketMessageCase.InvalidTicketMissing),
        BuildCase(CreateSupportTicketMessageCase.InvalidSenderMissing),
        BuildCase(CreateSupportTicketMessageCase.ExceptionPath)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases => NormalCases;

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateSupportTicketMessageCase.BoundaryDefault)
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateSupportTicketMessageCase.InvalidRequestShape),
        BuildCase(CreateSupportTicketMessageCase.InvalidDuplicateOrExisting),
        BuildCase(CreateSupportTicketMessageCase.InvalidTicketMissing),
        BuildCase(CreateSupportTicketMessageCase.InvalidSenderMissing)
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateSupportTicketMessageCase.ExceptionPath)
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

    private static UseCaseCaseSet BuildCase(CreateSupportTicketMessageCase caseId)
    {
        return caseId switch
        {
            CreateSupportTicketMessageCase.ValidDefault => CreateSuccessCase(
                "valid-default",
                UseCaseCaseKind.Valid,
                CreateRequest(caseId)),
            CreateSupportTicketMessageCase.BoundaryDefault => CreateSuccessCase(
                "boundary-default",
                UseCaseCaseKind.Boundary,
                CreateRequest(caseId)),
            CreateSupportTicketMessageCase.InvalidRequestShape => CreateCase(
                "invalid-request-shape",
                UseCaseCaseKind.Invalid,
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail,
                CreateRequest(caseId)),
            CreateSupportTicketMessageCase.InvalidDuplicateOrExisting => CreateFailureCase(
                "invalid-duplicate-message-existing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var messageRepositoryMock = new Mock<IGenericRepository<SupportTicketMessage, Guid>>(MockBehavior.Strict);
                    var ticketRepositoryMock = new Mock<IGenericRepository<SupportTicket, Guid>>(MockBehavior.Strict);
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                    SetupRepositories(unitOfWorkMock, messageRepositoryMock, ticketRepositoryMock, userRepositoryMock);

                    messageRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<SupportTicketMessage, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<SupportTicketMessage, object>>[]>()))
                        .ReturnsAsync(true);
                },
                expectedStatusCode: HttpStatusCode.BadRequest,
                expectedErrorCode: "TicketId.AlreadyExists"),
            CreateSupportTicketMessageCase.InvalidTicketMissing => CreateFailureCase(
                "invalid-ticket-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var messageRepositoryMock = new Mock<IGenericRepository<SupportTicketMessage, Guid>>(MockBehavior.Strict);
                    var ticketRepositoryMock = new Mock<IGenericRepository<SupportTicket, Guid>>(MockBehavior.Strict);
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                    SetupRepositories(unitOfWorkMock, messageRepositoryMock, ticketRepositoryMock, userRepositoryMock);

                    messageRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<SupportTicketMessage, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<SupportTicketMessage, object>>[]>()))
                        .ReturnsAsync(false);

                    ticketRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<SupportTicket, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<SupportTicket, object>>[]>()))
                        .ReturnsAsync(false);
                },
                expectedStatusCode: HttpStatusCode.NotFound,
                expectedErrorCode: " SupportTicket.NotFound"),
            CreateSupportTicketMessageCase.InvalidSenderMissing => CreateFailureCase(
                "invalid-sender-missing",
                UseCaseCaseKind.Invalid,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var messageRepositoryMock = new Mock<IGenericRepository<SupportTicketMessage, Guid>>(MockBehavior.Strict);
                    var ticketRepositoryMock = new Mock<IGenericRepository<SupportTicket, Guid>>(MockBehavior.Strict);
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                    SetupRepositories(unitOfWorkMock, messageRepositoryMock, ticketRepositoryMock, userRepositoryMock);

                    messageRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<SupportTicketMessage, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<SupportTicketMessage, object>>[]>()))
                        .ReturnsAsync(false);

                    ticketRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<SupportTicket, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<SupportTicket, object>>[]>()))
                        .ReturnsAsync(true);

                    userRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .ReturnsAsync(false);
                },
                expectedStatusCode: HttpStatusCode.NotFound,
                expectedErrorCode: " User.NotFound"),
            CreateSupportTicketMessageCase.ExceptionPath => CreateFailureCase(
                "exception-savechanges-throws",
                UseCaseCaseKind.Exception,
                CreateRequest(caseId),
                arrangeMocks: mocks =>
                {
                    var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                    var messageRepositoryMock = new Mock<IGenericRepository<SupportTicketMessage, Guid>>(MockBehavior.Strict);
                    var ticketRepositoryMock = new Mock<IGenericRepository<SupportTicket, Guid>>(MockBehavior.Strict);
                    var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                    SetupRepositories(unitOfWorkMock, messageRepositoryMock, ticketRepositoryMock, userRepositoryMock);

                    messageRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<SupportTicketMessage, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<SupportTicketMessage, object>>[]>()))
                        .ReturnsAsync(false);

                    ticketRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<SupportTicket, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<SupportTicket, object>>[]>()))
                        .ReturnsAsync(true);

                    userRepositoryMock
                        .Setup(repository => repository.AnyAsync(
                            It.IsAny<Expression<Func<User, bool>>>(),
                            It.IsAny<CancellationToken>(),
                            It.IsAny<Expression<Func<User, object>>[]>()))
                        .ReturnsAsync(true);

                    messageRepositoryMock
                        .Setup(repository => repository.Add(It.IsAny<SupportTicketMessage>()));

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
        global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand request)
    {
        SupportTicketMessage? addedMessage = null;

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
                        var messageRepositoryMock = new Mock<IGenericRepository<SupportTicketMessage, Guid>>(MockBehavior.Strict);
                        var ticketRepositoryMock = new Mock<IGenericRepository<SupportTicket, Guid>>(MockBehavior.Strict);
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);

                        SetupRepositories(unitOfWorkMock, messageRepositoryMock, ticketRepositoryMock, userRepositoryMock);

                        messageRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<SupportTicketMessage, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<SupportTicketMessage, object>>[]>()))
                            .ReturnsAsync(false);

                        ticketRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<SupportTicket, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<SupportTicket, object>>[]>()))
                            .ReturnsAsync(true);

                        userRepositoryMock
                            .Setup(repository => repository.AnyAsync(
                                It.IsAny<Expression<Func<User, bool>>>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(true);

                        messageRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<SupportTicketMessage>()))
                            .Callback<SupportTicketMessage>(entity => addedMessage = entity);

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<SupportTicketMessage>>(resultObject);

                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(addedMessage);
                        Assert.Equal(request.TicketId, addedMessage!.TicketId);
                        Assert.Equal(request.SenderId, addedMessage.SenderId);
                        Assert.Equal(request.Message, addedMessage.Message);
                        Assert.Equal(request.TicketId, result.Data?.TicketId);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateFailureCase(
        string name,
        UseCaseCaseKind kind,
        global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand request,
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
        Mock<IGenericRepository<SupportTicketMessage, Guid>> messageRepositoryMock,
        Mock<IGenericRepository<SupportTicket, Guid>> ticketRepositoryMock,
        Mock<IGenericRepository<User, Guid>> userRepositoryMock)
    {
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<SupportTicketMessage, Guid>())
            .Returns(messageRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<SupportTicket, Guid>())
            .Returns(ticketRepositoryMock.Object);
        unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
            .Returns(userRepositoryMock.Object);
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

    private static global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand CreateRequest(
        CreateSupportTicketMessageCase caseId)
    {
        return caseId switch
        {
            CreateSupportTicketMessageCase.ValidDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand
            {
                TicketId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SenderId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Message = "Need support on lesson booking."
            },
            CreateSupportTicketMessageCase.BoundaryDefault => new global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand
            {
                TicketId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                SenderId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Message = "?"
            },
            CreateSupportTicketMessageCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand
            {
                TicketId = Guid.Empty,
                SenderId = Guid.Empty,
                Message = string.Empty
            },
            CreateSupportTicketMessageCase.InvalidDuplicateOrExisting => new global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand
            {
                TicketId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                SenderId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Message = "Duplicate message"
            },
            CreateSupportTicketMessageCase.InvalidTicketMissing => new global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand
            {
                TicketId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                SenderId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Message = "Missing ticket branch"
            },
            CreateSupportTicketMessageCase.InvalidSenderMissing => new global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand
            {
                TicketId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                SenderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Message = "Missing sender branch"
            },
            CreateSupportTicketMessageCase.ExceptionPath => new global::EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage.CreateSupportTicketMessageCommand
            {
                TicketId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                SenderId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Message = "Exception branch"
            },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
