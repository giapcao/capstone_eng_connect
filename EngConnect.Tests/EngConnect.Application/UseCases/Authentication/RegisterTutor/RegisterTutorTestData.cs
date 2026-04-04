using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterTutor;

internal enum RegisterTutorCase
{
    ValidDefault,
    ValidWithoutIntroVideoFile,
    ValidWithoutCvFile,
    BoundaryDefault,
    InvalidRequestShape,
    InvalidNotFoundOrNull,
    ExceptionPath,
}

internal static class RegisterTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "RegisterTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterTutor/RegisterTutorCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterTutor/RegisterTutorCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Authentication/RegisterTutor/RegisterTutorCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(RegisterTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(RegisterTutorCase.ValidWithoutIntroVideoFile),
        BuildCase(RegisterTutorCase.ValidWithoutCvFile),
        BuildCase(RegisterTutorCase.BoundaryDefault),
        BuildCase(RegisterTutorCase.InvalidRequestShape),
        BuildCase(RegisterTutorCase.InvalidNotFoundOrNull),
        BuildCase(RegisterTutorCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(RegisterTutorCase.ValidDefault),
        BuildCase(RegisterTutorCase.ValidWithoutIntroVideoFile),
        BuildCase(RegisterTutorCase.ValidWithoutCvFile),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(RegisterTutorCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(RegisterTutorCase.InvalidRequestShape),
        BuildCase(RegisterTutorCase.InvalidNotFoundOrNull),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(RegisterTutorCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(RegisterTutorCase caseId)
    {
        return caseId switch
        {
            RegisterTutorCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RegisterTutorCase.ValidWithoutIntroVideoFile => CreateCase("valid-without-IntroVideoFile", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RegisterTutorCase.ValidWithoutCvFile => CreateCase("valid-without-CvFile", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RegisterTutorCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            RegisterTutorCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            RegisterTutorCase.InvalidNotFoundOrNull => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName("invalid-not-found-or-null"), CreateRequest(caseId)),
            RegisterTutorCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
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

    private static global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand CreateRequest(RegisterTutorCase caseId)
    {
        return caseId switch
        {
            RegisterTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            RegisterTutorCase.ValidWithoutIntroVideoFile => new global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = null, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            RegisterTutorCase.ValidWithoutCvFile => new global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = null },
            RegisterTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            RegisterTutorCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand { UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Headline = "", Bio = "", MonthExperience = 0, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            RegisterTutorCase.InvalidNotFoundOrNull => new global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            RegisterTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Authentication.RegisterTutor.RegisterTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}