using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.CreateTutor;

internal enum CreateTutorCase
{
    ValidDefault,
    ValidWithoutIntroVideoFile,
    ValidWithoutCvFile,
    BoundaryDefault,
    InvalidRequestShape,
    ExceptionPath,
}

internal static class CreateTutorTestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "CreateTutor",
        RequestTypeFullName = "EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand",
        HandlerTypeFullName = "EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommandHandler",
        ValidatorTypeFullName = "EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommandValidator",
        RequestSourceRelativePath = "EngConnect.Application/UseCases/Tutors/CreateTutor/CreateTutorCommand.cs",
        HandlerSourceRelativePath = "EngConnect.Application/UseCases/Tutors/CreateTutor/CreateTutorCommandHandler.cs",
        ValidatorSourceRelativePath = "EngConnect.Application/UseCases/Tutors/CreateTutor/CreateTutorCommandValidator.cs"
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
        BuildCase(CreateTutorCase.ValidDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
        BuildCase(CreateTutorCase.ValidWithoutIntroVideoFile),
        BuildCase(CreateTutorCase.ValidWithoutCvFile),
        BuildCase(CreateTutorCase.BoundaryDefault),
        BuildCase(CreateTutorCase.InvalidRequestShape),
        BuildCase(CreateTutorCase.ExceptionPath),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
        BuildCase(CreateTutorCase.ValidDefault),
        BuildCase(CreateTutorCase.ValidWithoutIntroVideoFile),
        BuildCase(CreateTutorCase.ValidWithoutCvFile),
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
        BuildCase(CreateTutorCase.BoundaryDefault),
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
        BuildCase(CreateTutorCase.InvalidRequestShape),
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
        BuildCase(CreateTutorCase.ExceptionPath),
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

    private static UseCaseCaseSet BuildCase(CreateTutorCase caseId)
    {
        return caseId switch
        {
            CreateTutorCase.ValidDefault => CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateTutorCase.ValidWithoutIntroVideoFile => CreateCase("valid-without-IntroVideoFile", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateTutorCase.ValidWithoutCvFile => CreateCase("valid-without-CvFile", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateTutorCase.BoundaryDefault => CreateCase("boundary-default", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass, CreateRequest(caseId)),
            CreateTutorCase.InvalidRequestShape => CreateCase("invalid-request-shape", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Fail, CreateRequest(caseId)),
            CreateTutorCase.ExceptionPath => CreateCase("exception-path", UseCaseCaseKind.Exception, UseCaseHandlerExpectation.Exception, UseCaseValidatorExpectation.Skip, CreateRequest(caseId)),
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

    private static global::EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand CreateRequest(CreateTutorCase caseId)
    {
        return caseId switch
        {
            CreateTutorCase.ValidDefault => new global::EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            CreateTutorCase.ValidWithoutIntroVideoFile => new global::EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = null, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            CreateTutorCase.ValidWithoutCvFile => new global::EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = null },
            CreateTutorCase.BoundaryDefault => new global::EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            CreateTutorCase.InvalidRequestShape => new global::EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand { UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"), Headline = "", Bio = "", MonthExperience = 0, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 0, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            CreateTutorCase.ExceptionPath => new global::EngConnect.Application.UseCases.Tutors.CreateTutor.CreateTutorCommand { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Headline = "Experienced English Tutor", Bio = "Sample description", MonthExperience = 1, IntroVideoFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) }, CvFile = new global::EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload { FileName = "test.txt", ContentType = "text/plain", Length = 11, Content = new MemoryStream(Encoding.UTF8.GetBytes("hello world")) } },
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}