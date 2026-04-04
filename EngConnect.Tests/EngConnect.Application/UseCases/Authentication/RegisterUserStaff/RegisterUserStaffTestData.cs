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
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUserStaff;

internal enum RegisterUserStaffCase
{
    ValidDefault,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidIsEmailExistExisting,
    InvalidIsUsernameExistExisting,
    ExceptionPath,
}

internal static class RegisterUserStaffTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "RegisterUserStaff",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterUserStaff/RegisterUserStaffCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterUserStaff/RegisterUserStaffCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterUserStaff/RegisterUserStaffCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(RegisterUserStaffCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(RegisterUserStaffCase.BoundaryDefault),
        BuildCase(RegisterUserStaffCase.InvalidRequestShape),
        BuildCase(RegisterUserStaffCase.InvalidIsEmailExistExisting),
        BuildCase(RegisterUserStaffCase.InvalidIsUsernameExistExisting),
        BuildCase(RegisterUserStaffCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(RegisterUserStaffCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(RegisterUserStaffCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(RegisterUserStaffCase.InvalidRequestShape),
        BuildCase(RegisterUserStaffCase.InvalidIsEmailExistExisting),
        BuildCase(RegisterUserStaffCase.InvalidIsUsernameExistExisting),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(RegisterUserStaffCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(RegisterUserStaffCase caseId)
    {
        return caseId switch
        {
            RegisterUserStaffCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RegisterUserStaffCase.BoundaryDefault => CreateSuccessCase("boundary-role-missing", CreateRequest(caseId), roleExists: false),
            RegisterUserStaffCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            RegisterUserStaffCase.InvalidIsEmailExistExisting => CreateCase("invalid-isEmailExist-existing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RegisterUserStaffCase.InvalidIsUsernameExistExisting => CreateCase("invalid-isUsernameExist-existing", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RegisterUserStaffCase.ExceptionPath => CreateExceptionCase(CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }

    private static UseCaseCaseSet CreateSuccessCase(
        string name,
        global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand request,
        bool roleExists)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = UseCaseCaseKind.Boundary,
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
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                        var roleRepositoryMock = new Mock<IGenericRepository<Role, Guid>>(MockBehavior.Strict);
                        var userRoleRepositoryMock = new Mock<IGenericRepository<UserRole, Guid>>(MockBehavior.Strict);
                        var transactionMock = CreateTransactionMock(Guid.Parse("90000000-0000-0000-0000-000000000001"));

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                            .Returns(userRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Role, Guid>())
                            .Returns(roleRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<UserRole, Guid>())
                            .Returns(userRoleRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                            .ReturnsAsync(transactionMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ReturnsAsync(1);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.CommitTransactionAsync())
                            .Returns(Task.CompletedTask);

                        userRepositoryMock
                            .Setup(repository => repository.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(false);
                        userRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<User>()));

                        roleRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Role, object>>[]>()))
                            .ReturnsAsync(roleExists
                                ? new Role
                                {
                                    Id = Guid.Parse("90000000-0000-0000-0000-000000000002"),
                                    Code = "Staff"
                                }
                                : null);

                        if (roleExists)
                        {
                            userRoleRepositoryMock
                                .Setup(repository => repository.Add(It.IsAny<UserRole>()));
                        }
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<global::EngConnect.Application.UseCases.Authentication.Common.RegisterUserStaffResponse>>(resultObject);
                        Assert.True(result.IsSuccess);
                        Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
                        Assert.NotNull(result.Data);
                        Assert.Equal(request.Email, result.Data!.Email);
                        Assert.Equal("Active", result.Data.Status);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static UseCaseCaseSet CreateExceptionCase(
        global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand request)
    {
        return new UseCaseCaseSet
        {
            Name = "exception-after-transaction-started",
            Kind = UseCaseCaseKind.Exception,
            HandlerExpectation = UseCaseHandlerExpectation.Failure,
            ValidatorExpectation = UseCaseValidatorExpectation.Skip,
            TestCase = new UseCaseTestCase
            {
                Name = "exception-after-transaction-started",
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    ArrangeMocks = mocks =>
                    {
                        var unitOfWorkMock = mocks.StrictMock<IUnitOfWork>();
                        var userRepositoryMock = new Mock<IGenericRepository<User, Guid>>(MockBehavior.Strict);
                        var roleRepositoryMock = new Mock<IGenericRepository<Role, Guid>>(MockBehavior.Strict);
                        var userRoleRepositoryMock = new Mock<IGenericRepository<UserRole, Guid>>(MockBehavior.Strict);
                        var transactionMock = CreateTransactionMock(Guid.Parse("90000000-0000-0000-0000-000000000003"));

                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<User, Guid>())
                            .Returns(userRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<Role, Guid>())
                            .Returns(roleRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.GetRepository<UserRole, Guid>())
                            .Returns(userRoleRepositoryMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.BeginTransactionAsync(It.IsAny<System.Data.IsolationLevel>()))
                            .ReturnsAsync(transactionMock.Object);
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(true, true))
                            .ThrowsAsync(new InvalidOperationException("save failed"));
                        unitOfWorkMock
                            .Setup(unitOfWork => unitOfWork.RollbackTransactionAsync())
                            .Returns(Task.CompletedTask);

                        userRepositoryMock
                            .Setup(repository => repository.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>[]>()))
                            .ReturnsAsync(false);
                        userRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<User>()));

                        roleRepositoryMock
                            .Setup(repository => repository.FindFirstAsync(
                                It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(),
                                It.IsAny<bool>(),
                                It.IsAny<CancellationToken>(),
                                It.IsAny<System.Linq.Expressions.Expression<Func<Role, object>>[]>()))
                            .ReturnsAsync(new Role
                            {
                                Id = Guid.Parse("90000000-0000-0000-0000-000000000004"),
                                Code = "Staff"
                            });

                        userRoleRepositoryMock
                            .Setup(repository => repository.Add(It.IsAny<UserRole>()));
                    },
                    AssertHandlerResultAsync = resultObject =>
                    {
                        var result = Assert.IsType<Result<global::EngConnect.Application.UseCases.Authentication.Common.RegisterUserStaffResponse>>(resultObject);
                        Assert.True(result.IsFailure);
                        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatusCode);
                        Assert.Equal("Server.Error", result.Error?.Code);
                        Assert.Null(result.Data);
                        return Task.CompletedTask;
                    }
                }
            }
        };
    }

    private static Mock<IDbContextTransaction> CreateTransactionMock(Guid transactionId)
    {
        var transactionMock = new Mock<IDbContextTransaction>(MockBehavior.Strict);
        transactionMock.SetupGet(transaction => transaction.TransactionId).Returns(transactionId);
        return transactionMock;
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

    private static global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand CreateRequest(RegisterUserStaffCase caseId)
    {
        return caseId switch
        {
            RegisterUserStaffCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand { FirstName = "Sample", LastName = "Tester", Email = "registeruserstaffcommand@example.com", UserName = "sample.user", Phone = "+84987654321", AddressNum = "Sample description", ProvinceId = "SampleValue", ProvinceName = "Sample", WardId = "SampleValue", WardName = "Sample", Password = "P@ssw0rd!" },
            RegisterUserStaffCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand { FirstName = "Sample", LastName = "Tester", Email = "registeruserstaffcommand@example.com", UserName = "sample.user", Phone = "+84987654321", AddressNum = "Sample description", ProvinceId = "SampleValue", ProvinceName = "Sample", WardId = "SampleValue", WardName = "Sample", Password = "P@ssw0rd!" },
            RegisterUserStaffCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand { FirstName = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", LastName = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Email = "", UserName = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Phone = "invalid-phone", AddressNum = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", ProvinceId = "", ProvinceName = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", WardId = "", WardName = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", Password = "" },
            RegisterUserStaffCase.InvalidIsEmailExistExisting => new global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand { FirstName = "Sample", LastName = "Tester", Email = "tester@example.com", UserName = "sample.user", Phone = "+84987654321", AddressNum = "Sample description", ProvinceId = "SampleValue", ProvinceName = "Sample", WardId = "SampleValue", WardName = "Sample", Password = "P@ssw0rd!" },
            RegisterUserStaffCase.InvalidIsUsernameExistExisting => new global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand { FirstName = "Sample", LastName = "Tester", Email = "registeruserstaffcommand@example.com", UserName = "seed-value", Phone = "+84987654321", AddressNum = "Sample description", ProvinceId = "SampleValue", ProvinceName = "Sample", WardId = "SampleValue", WardName = "Sample", Password = "P@ssw0rd!" },
            RegisterUserStaffCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.RegisterUserStaff.RegisterUserStaffCommand { FirstName = "Sample", LastName = "Tester", Email = "registeruserstaffcommand@example.com", UserName = "sample.user", Phone = "+84987654321", AddressNum = "Sample description", ProvinceId = "SampleValue", ProvinceName = "Sample", WardId = "SampleValue", WardName = "Sample", Password = "P@ssw0rd!" },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
