using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using EngConnect.Tests.Common;

var repoRoot = args.Length > 0 ? args[0] : FindRepoRoot(Directory.GetCurrentDirectory());
var applicationUseCasesRoot = Path.Combine(repoRoot, "EngConnect.Application", "UseCases");
var testUseCasesRoot = Path.Combine(repoRoot, "EngConnect.Tests", "EngConnect.Application", "UseCases");
var excludedKeywords = new[] { "course", "module", "session", "resource" };

foreach (var directory in Directory.EnumerateDirectories(applicationUseCasesRoot, "*", SearchOption.AllDirectories)
             .Where(path => IsIncludedPath(path, excludedKeywords))
             .OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
{
    var handlerFile = Directory.EnumerateFiles(directory, "*Handler.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();
    var validatorFile = Directory.EnumerateFiles(directory, "*Validator.cs", SearchOption.TopDirectoryOnly).FirstOrDefault();

    if (handlerFile is null && validatorFile is null)
    {
        continue;
    }

    var handlerMetadata = handlerFile is null ? null : GetHandlerMetadata(handlerFile);
    var validatorMetadata = validatorFile is null ? null : GetValidatorMetadata(validatorFile);
    var requestType = handlerMetadata?.RequestType ?? validatorMetadata?.RequestType;
    if (string.IsNullOrWhiteSpace(requestType))
    {
        continue;
    }

    var requestFilePath = GetRequestFilePath(directory, requestType);
    if (requestFilePath is null)
    {
        continue;
    }

    var relativePath = Path.GetRelativePath(applicationUseCasesRoot, directory);
    var dottedRelativePath = relativePath.Replace(Path.DirectorySeparatorChar, '.');
    var requestTypeFullName = $"EngConnect.Application.UseCases.{dottedRelativePath}.{requestType}";
    var handlerTypeFullName = handlerMetadata is null ? null : $"EngConnect.Application.UseCases.{dottedRelativePath}.{handlerMetadata.TypeName}";
    var validatorTypeFullName = validatorMetadata is null ? null : $"EngConnect.Application.UseCases.{dottedRelativePath}.{validatorMetadata.TypeName}";
    var definition = new UseCaseDefinition
    {
        UseCaseName = Path.GetFileName(directory),
        RequestTypeFullName = requestTypeFullName,
        HandlerTypeFullName = handlerTypeFullName,
        ValidatorTypeFullName = validatorTypeFullName,
        RequestSourceRelativePath = ToRelativeRepoPath(repoRoot, requestFilePath),
        HandlerSourceRelativePath = handlerFile is null ? null : ToRelativeRepoPath(repoRoot, handlerFile),
        ValidatorSourceRelativePath = validatorFile is null ? null : ToRelativeRepoPath(repoRoot, validatorFile)
    };

    var catalog = UseCaseCaseCatalogFactory.CreateCatalog(definition);
    var validCases = CreateBlueprints(catalog.ValidCases);
    var boundaryCases = CreateBlueprints(catalog.BoundaryCases);
    var invalidCases = CreateBlueprints(catalog.InvalidCases);
    var exceptionCases = CreateBlueprints(catalog.ExceptionCases);

    var testDirectory = Path.Combine(testUseCasesRoot, relativePath);
    Directory.CreateDirectory(testDirectory);

    File.WriteAllText(
        Path.Combine(testDirectory, $"{definition.UseCaseName}TestData.cs"),
        GenerateTestDataContent(definition, dottedRelativePath, validCases, boundaryCases, invalidCases, exceptionCases),
        Encoding.UTF8);

    File.WriteAllText(
        Path.Combine(testDirectory, $"{definition.UseCaseName}HandlerTests.cs"),
        handlerTypeFullName is null
            ? GenerateHandlerPlaceholderContent(definition.UseCaseName, dottedRelativePath)
            : GenerateHandlerTestsContent(definition.UseCaseName, dottedRelativePath),
        Encoding.UTF8);

    File.WriteAllText(
        Path.Combine(testDirectory, $"{definition.UseCaseName}ValidatorTests.cs"),
        validatorTypeFullName is null
            ? GenerateValidatorPlaceholderContent(definition.UseCaseName, dottedRelativePath)
            : GenerateValidatorTestsContent(definition.UseCaseName, dottedRelativePath),
        Encoding.UTF8);

    File.WriteAllText(
        Path.Combine(testDirectory, $"{definition.UseCaseName}BranchTests.cs"),
        GenerateBranchTestsContent(
            definition.UseCaseName,
            dottedRelativePath,
            hasHandler: handlerTypeFullName is not null,
            hasValidator: validatorTypeFullName is not null),
        Encoding.UTF8);
}

DeleteLegacyTests(testUseCasesRoot, "*ValidTests.cs");
DeleteLegacyTests(testUseCasesRoot, "*InvalidTests.cs");
return;

static string FindRepoRoot(string startDirectory)
{
    var current = new DirectoryInfo(startDirectory);
    while (current is not null)
    {
        if (Directory.Exists(Path.Combine(current.FullName, "EngConnect.Application"))
            && Directory.Exists(Path.Combine(current.FullName, "EngConnect.Tests")))
        {
            return current.FullName;
        }

        current = current.Parent;
    }

    throw new InvalidOperationException("Cannot locate repository root.");
}

static bool IsIncludedPath(string path, IEnumerable<string> excludedKeywords)
{
    var candidate = path.ToLowerInvariant();
    return excludedKeywords.All(keyword => !candidate.Contains(keyword, StringComparison.Ordinal));
}

static UseCaseTypeMetadata? GetHandlerMetadata(string filePath)
{
    var content = File.ReadAllText(filePath);
    var match = Regex.Match(content, @"class\s+(?<type>\w+)\s*:\s*I(?:CommandHandler|QueryHandler)<(?<request>[^,>]+)");
    return match.Success
        ? new UseCaseTypeMetadata(match.Groups["type"].Value.Trim(), match.Groups["request"].Value.Trim())
        : null;
}

static UseCaseTypeMetadata? GetValidatorMetadata(string filePath)
{
    var content = File.ReadAllText(filePath);
    var match = Regex.Match(content, @"class\s+(?<type>\w+)\s*:\s*AbstractValidator<(?<request>[^>]+)>");
    return match.Success
        ? new UseCaseTypeMetadata(match.Groups["type"].Value.Trim(), match.Groups["request"].Value.Trim())
        : null;
}

static string? GetRequestFilePath(string directory, string requestType)
{
    var exactPath = Path.Combine(directory, $"{requestType}.cs");
    if (File.Exists(exactPath))
    {
        return exactPath;
    }

    return Directory.EnumerateFiles(directory, "*.cs", SearchOption.TopDirectoryOnly)
        .FirstOrDefault(path => string.Equals(Path.GetFileNameWithoutExtension(path), requestType, StringComparison.Ordinal));
}

static string ToRelativeRepoPath(string repoRoot, string fullPath)
{
    return Path.GetRelativePath(repoRoot, fullPath).Replace('\\', '/');
}

static List<CaseBlueprint> CreateBlueprints(IReadOnlyList<UseCaseCaseSet> cases)
{
    var result = new List<CaseBlueprint>(cases.Count);
    var usedNames = new Dictionary<string, int>(StringComparer.Ordinal);

    for (var index = 0; index < cases.Count; index++)
    {
        var @case = cases[index];
        var enumName = ToEnumMemberName(@case.Name, index);
        if (usedNames.TryGetValue(enumName, out var count))
        {
            count++;
            usedNames[enumName] = count;
            enumName += count.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            usedNames[enumName] = 1;
        }

        var scenario = @case.TestCase.Scenario;
        var useTemplateScenario = scenario.ArrangeMocks is not null
                                  || scenario.AssertHandlerResultAsync is not null
                                  || scenario.AssertValidatorResultAsync is not null;

        result.Add(new CaseBlueprint(
            enumName,
            @case.Name,
            SerializeValue(@case.TestCase.Scenario.Request),
            @case.Kind,
            @case.HandlerExpectation,
            @case.ValidatorExpectation,
            useTemplateScenario));
    }

    return result;
}

static string ToEnumMemberName(string value, int index)
{
    var matches = Regex.Matches(value, "[A-Za-z0-9]+");
    if (matches.Count == 0)
    {
        return $"Case{index}";
    }

    var builder = new StringBuilder();
    foreach (Match match in matches)
    {
        var token = match.Value;
        builder.Append(token.Length == 1
            ? token.ToUpperInvariant()
            : char.ToUpperInvariant(token[0]) + token[1..]);
    }

    var enumName = builder.ToString();
    return char.IsDigit(enumName[0]) ? $"Case{enumName}" : enumName;
}

static string GenerateTestDataContent(
    UseCaseDefinition definition,
    string dottedRelativePath,
    IReadOnlyList<CaseBlueprint> validCases,
    IReadOnlyList<CaseBlueprint> boundaryCases,
    IReadOnlyList<CaseBlueprint> invalidCases,
    IReadOnlyList<CaseBlueprint> exceptionCases)
{
    var enumTypeName = $"{definition.UseCaseName}Case";
    var allCases = validCases.Concat(boundaryCases).Concat(invalidCases).Concat(exceptionCases).ToList();
    var primaryValidCase = validCases.FirstOrDefault(@case =>
                               string.Equals(@case.TemplateName, "valid-default", StringComparison.Ordinal))
                           ?? validCases.FirstOrDefault();
    var normalCases = primaryValidCase is null ? [] : new[] { primaryValidCase };
    var branchCases = allCases
        .Where(@case => primaryValidCase is null || !string.Equals(@case.EnumName, primaryValidCase.EnumName, StringComparison.Ordinal))
        .ToList();

    var enumLines = string.Join(Environment.NewLine, allCases.Select(@case => $"    {@case.EnumName},"));
    var normalLines = string.Join(Environment.NewLine, normalCases.Select(@case => $"        BuildCase({enumTypeName}.{@case.EnumName}),"));
    var branchLines = string.Join(Environment.NewLine, branchCases.Select(@case => $"        BuildCase({enumTypeName}.{@case.EnumName}),"));
    var validLines = string.Join(Environment.NewLine, validCases.Select(@case => $"        BuildCase({enumTypeName}.{@case.EnumName}),"));
    var boundaryLines = string.Join(Environment.NewLine, boundaryCases.Select(@case => $"        BuildCase({enumTypeName}.{@case.EnumName}),"));
    var invalidLines = string.Join(Environment.NewLine, invalidCases.Select(@case => $"        BuildCase({enumTypeName}.{@case.EnumName}),"));
    var exceptionLines = string.Join(Environment.NewLine, exceptionCases.Select(@case => $"        BuildCase({enumTypeName}.{@case.EnumName}),"));
    var requestMappings = string.Join(Environment.NewLine, allCases.Select(@case => $"            {enumTypeName}.{@case.EnumName} => {@case.RequestCode},"));
    var buildCaseMappings = string.Join(Environment.NewLine, allCases.Select(@case => @case.UseTemplateScenario
        ? $"            {enumTypeName}.{@case.EnumName} => UseCaseCaseSetFactory.CloneWithRequest(TemplateCatalog.GetCaseByName(\"{EscapeVerbatim(@case.TemplateName)}\"), CreateRequest(caseId)),"
        : $"            {enumTypeName}.{@case.EnumName} => CreateCase(\"{EscapeVerbatim(@case.TemplateName)}\", UseCaseCaseKind.{@case.Kind}, UseCaseHandlerExpectation.{@case.HandlerExpectation}, UseCaseValidatorExpectation.{@case.ValidatorExpectation}, CreateRequest(caseId)),"));
    var testNamespace = $"EngConnect.Tests.EngConnect.Application.UseCases.{dottedRelativePath}";

    return $$"""
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EngConnect.Tests.Common;

namespace {{testNamespace}};

internal enum {{enumTypeName}}
{
{{enumLines}}
}

internal static class {{definition.UseCaseName}}TestData
{
    public static readonly UseCaseDefinition Definition = new()
    {
        UseCaseName = "{{definition.UseCaseName}}",
        RequestTypeFullName = "{{definition.RequestTypeFullName}}",
        HandlerTypeFullName = {{ToNullableLiteral(definition.HandlerTypeFullName)}},
        ValidatorTypeFullName = {{ToNullableLiteral(definition.ValidatorTypeFullName)}},
        RequestSourceRelativePath = "{{definition.RequestSourceRelativePath}}",
        HandlerSourceRelativePath = {{ToNullableLiteral(definition.HandlerSourceRelativePath)}},
        ValidatorSourceRelativePath = {{ToNullableLiteral(definition.ValidatorSourceRelativePath)}}
    };

    private static readonly UseCaseCaseCatalog TemplateCatalog = UseCaseCaseCatalogFactory.CreateCatalog(Definition);

    public static IReadOnlyList<UseCaseCaseSet> NormalCases { get; } =
    [
{{normalLines}}
    ];

    public static IReadOnlyList<UseCaseCaseSet> BranchCases { get; } =
    [
{{branchLines}}
    ];

    public static IReadOnlyList<UseCaseCaseSet> ValidCases { get; } =
    [
{{validLines}}
    ];

    public static IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; } =
    [
{{boundaryLines}}
    ];

    public static IReadOnlyList<UseCaseCaseSet> InvalidCases { get; } =
    [
{{invalidLines}}
    ];

    public static IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; } =
    [
{{exceptionLines}}
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

    private static UseCaseCaseSet BuildCase({{enumTypeName}} caseId)
    {
        return caseId switch
        {
{{buildCaseMappings}}
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

    private static global::{{definition.RequestTypeFullName}} CreateRequest({{enumTypeName}} caseId)
    {
        return caseId switch
        {
{{requestMappings}}
            _ => throw new ArgumentOutOfRangeException(nameof(caseId), caseId, null)
        };
    }
}
""";
}

static string GenerateHandlerTestsContent(string useCaseName, string dottedRelativePath)
{
    var testNamespace = $"EngConnect.Tests.EngConnect.Application.UseCases.{dottedRelativePath}";
    return $$"""
using EngConnect.Tests.Common;
using Xunit;

namespace {{testNamespace}};

public class {{useCaseName}}HandlerTests
{
    [Theory]
    [MemberData(nameof({{useCaseName}}TestData.NormalHandlerCases), MemberType = typeof({{useCaseName}}TestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync({{useCaseName}}TestData.Definition, caseSet);
    }
}
""";
}

static string GenerateHandlerPlaceholderContent(string useCaseName, string dottedRelativePath)
{
    var testNamespace = $"EngConnect.Tests.EngConnect.Application.UseCases.{dottedRelativePath}";
    return $$"""
using Xunit;

namespace {{testNamespace}};

public class {{useCaseName}}HandlerTests
{
    [Fact]
    public void HandleAsync_has_no_handler_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace({{useCaseName}}TestData.Definition.HandlerTypeFullName));
    }
}
""";
}

static string GenerateValidatorTestsContent(string useCaseName, string dottedRelativePath)
{
    var testNamespace = $"EngConnect.Tests.EngConnect.Application.UseCases.{dottedRelativePath}";
    return $$"""
using EngConnect.Tests.Common;
using Xunit;

namespace {{testNamespace}};

public class {{useCaseName}}ValidatorTests
{
    [Theory]
    [MemberData(nameof({{useCaseName}}TestData.NormalValidatorCases), MemberType = typeof({{useCaseName}}TestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync({{useCaseName}}TestData.Definition, caseSet);
    }
}
""";
}

static string GenerateBranchTestsContent(string useCaseName, string dottedRelativePath, bool hasHandler, bool hasValidator)
{
    var testNamespace = $"EngConnect.Tests.EngConnect.Application.UseCases.{dottedRelativePath}";
    var members = new List<string>();

    if (hasHandler)
    {
        members.Add($$"""
    [Theory]
    [MemberData(nameof({{useCaseName}}TestData.HandlerBranchCases), MemberType = typeof({{useCaseName}}TestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync({{useCaseName}}TestData.Definition, caseSet);
    }
""");
    }
    else
    {
        members.Add($$"""
    [Fact]
    public void HandleAsync_has_no_handler_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace({{useCaseName}}TestData.Definition.HandlerTypeFullName));
    }
""");
    }

    if (hasValidator)
    {
        members.Add($$"""
    [Theory]
    [MemberData(nameof({{useCaseName}}TestData.ValidatorBranchCases), MemberType = typeof({{useCaseName}}TestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync({{useCaseName}}TestData.Definition, caseSet);
    }
""");
    }
    else
    {
        members.Add($$"""
    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace({{useCaseName}}TestData.Definition.ValidatorTypeFullName));
    }
""");
    }

    var body = string.Join(Environment.NewLine + Environment.NewLine, members);

    return $$"""
using EngConnect.Tests.Common;
using Xunit;

namespace {{testNamespace}};

public class {{useCaseName}}BranchTests
{
{{body}}
}
""";
}

static string GenerateValidatorPlaceholderContent(string useCaseName, string dottedRelativePath)
{
    var testNamespace = $"EngConnect.Tests.EngConnect.Application.UseCases.{dottedRelativePath}";
    return $$"""
using Xunit;

namespace {{testNamespace}};

public class {{useCaseName}}ValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace({{useCaseName}}TestData.Definition.ValidatorTypeFullName));
    }
}
""";
}

static void DeleteLegacyTests(string rootDirectory, string searchPattern)
{
    foreach (var file in Directory.EnumerateFiles(rootDirectory, searchPattern, SearchOption.AllDirectories))
    {
        File.Delete(file);
    }
}

static string SerializeValue(object? value)
{
    if (value is null)
    {
        return "null";
    }

    var type = value.GetType();

    if (type == typeof(string))
    {
        return ToStringLiteral((string)value);
    }

    if (type == typeof(bool))
    {
        return (bool)value ? "true" : "false";
    }

    if (type == typeof(int) || type == typeof(short) || type == typeof(byte))
    {
        return Convert.ToString(value, CultureInfo.InvariantCulture)!;
    }

    if (type == typeof(long))
    {
        return $"{Convert.ToString(value, CultureInfo.InvariantCulture)}L";
    }

    if (type == typeof(decimal))
    {
        return $"{((decimal)value).ToString(CultureInfo.InvariantCulture)}m";
    }

    if (type == typeof(double))
    {
        return $"{((double)value).ToString(CultureInfo.InvariantCulture)}d";
    }

    if (type == typeof(float))
    {
        return $"{((float)value).ToString(CultureInfo.InvariantCulture)}f";
    }

    if (type == typeof(Guid))
    {
        return $"Guid.Parse({ToStringLiteral(((Guid)value).ToString())})";
    }

    if (type == typeof(DateTime))
    {
        var dateTime = (DateTime)value;
        return $"new DateTime({dateTime.Year}, {dateTime.Month}, {dateTime.Day}, {dateTime.Hour}, {dateTime.Minute}, {dateTime.Second}, DateTimeKind.{dateTime.Kind})";
    }

    if (type == typeof(DateTimeOffset))
    {
        var dateTimeOffset = (DateTimeOffset)value;
        return $"new DateTimeOffset({dateTimeOffset.Year}, {dateTimeOffset.Month}, {dateTimeOffset.Day}, {dateTimeOffset.Hour}, {dateTimeOffset.Minute}, {dateTimeOffset.Second}, new TimeSpan({dateTimeOffset.Offset.Hours}, {dateTimeOffset.Offset.Minutes}, {dateTimeOffset.Offset.Seconds}))";
    }

    if (type.FullName == "System.DateOnly")
    {
        dynamic dateOnly = value;
        return $"new DateOnly({dateOnly.Year}, {dateOnly.Month}, {dateOnly.Day})";
    }

    if (type.FullName == "System.TimeOnly")
    {
        dynamic timeOnly = value;
        return $"new TimeOnly({timeOnly.Hour}, {timeOnly.Minute}, {timeOnly.Second})";
    }

    if (type.IsEnum)
    {
        return $"{FormatTypeName(type)}.{value}";
    }

    if (type == typeof(byte[]))
    {
        var items = ((byte[])value).Select(item => item.ToString(CultureInfo.InvariantCulture));
        return $"new byte[] {{ {string.Join(", ", items)} }}";
    }

    if (typeof(ClaimsPrincipal).IsAssignableFrom(type))
    {
        return "TestObjectFactory.CreatePrincipal()";
    }

    if (type.FullName == "Microsoft.AspNetCore.Http.FormFile"
        || type.GetInterface("Microsoft.AspNetCore.Http.IFormFile") is not null)
    {
        var fileName = (string?)type.GetProperty("FileName")?.GetValue(value) ?? "test.txt";
        var contentType = (string?)type.GetProperty("ContentType")?.GetValue(value) ?? "text/plain";
        return $"TestObjectFactory.CreateFormFile({ToStringLiteral(fileName)}, {ToStringLiteral(contentType)}, \"hello world\")";
    }

    if (typeof(Stream).IsAssignableFrom(type))
    {
        return "new MemoryStream(Encoding.UTF8.GetBytes(\"hello world\"))";
    }

    if (type.FullName == "EngConnect.BuildingBlock.Contracts.Models.Files.FileUpload")
    {
        var fileName = (string?)type.GetProperty("FileName")?.GetValue(value) ?? "test.txt";
        var contentType = (string?)type.GetProperty("ContentType")?.GetValue(value) ?? "text/plain";
        var length = Convert.ToString(type.GetProperty("Length")?.GetValue(value), CultureInfo.InvariantCulture) ?? "0";
        return $"new {FormatTypeName(type)} {{ FileName = {ToStringLiteral(fileName)}, ContentType = {ToStringLiteral(contentType)}, Length = {length}, Content = new MemoryStream(Encoding.UTF8.GetBytes(\"hello world\")) }}";
    }

    if (type.IsArray)
    {
        var items = ((IEnumerable)value).Cast<object?>().Select(SerializeValue);
        return $"new {FormatTypeName(type)} {{ {string.Join(", ", items)} }}";
    }

    if (type.IsGenericType)
    {
        var definition = type.GetGenericTypeDefinition();
        if (definition == typeof(List<>)
            || definition == typeof(IList<>)
            || definition == typeof(ICollection<>)
            || definition == typeof(IEnumerable<>)
            || definition == typeof(IReadOnlyList<>))
        {
            var items = ((IEnumerable)value).Cast<object?>().Select(SerializeValue);
            return $"new {FormatTypeName(type)} {{ {string.Join(", ", items)} }}";
        }

        if (definition == typeof(Dictionary<,>)
            || definition == typeof(IDictionary<,>)
            || definition == typeof(IReadOnlyDictionary<,>))
        {
            var pairs = ((IEnumerable)value).Cast<object>()
                .Select(item =>
                {
                    var pairType = item.GetType();
                    var key = pairType.GetProperty("Key")!.GetValue(item);
                    var pairValue = pairType.GetProperty("Value")!.GetValue(item);
                    return $"{{ {SerializeValue(key)}, {SerializeValue(pairValue)} }}";
                });
            return $"new {FormatTypeName(type)} {{ {string.Join(", ", pairs)} }}";
        }
    }

    return SerializeObject(type, value);
}

static string SerializeObject(Type type, object value)
{
    var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
    var properties = GetSerializableProperties(type).ToArray();
    var propertyMap = properties.ToDictionary(property => property.Name, StringComparer.OrdinalIgnoreCase);

    var parameterlessConstructor = constructors.FirstOrDefault(constructor => constructor.GetParameters().Length == 0);
    if (parameterlessConstructor is not null)
    {
        var assignments = properties.Select(property => $"{property.Name} = {SerializeValue(property.GetValue(value))}");
        return $"new {FormatTypeName(type)} {{ {string.Join(", ", assignments)} }}";
    }

    var constructor = constructors
        .OrderByDescending(candidate => candidate.GetParameters().Length)
        .FirstOrDefault(candidate => candidate.GetParameters().All(parameter => propertyMap.ContainsKey(parameter.Name!)));

    if (constructor is null)
    {
        throw new InvalidOperationException($"Cannot serialize type {type.FullName} because no usable constructor was found.");
    }

    var consumed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var arguments = constructor.GetParameters()
        .Select(parameter =>
        {
            var property = propertyMap[parameter.Name!];
            consumed.Add(property.Name);
            return SerializeValue(property.GetValue(value));
        });

    var expression = $"new {FormatTypeName(type)}({string.Join(", ", arguments)})";
    var remainingAssignments = properties
        .Where(property => !consumed.Contains(property.Name) && property.CanWrite)
        .Select(property => $"{property.Name} = {SerializeValue(property.GetValue(value))}")
        .ToArray();

    return remainingAssignments.Length == 0
        ? expression
        : $"{expression} {{ {string.Join(", ", remainingAssignments)} }}";
}

static IEnumerable<PropertyInfo> GetSerializableProperties(Type type)
{
    return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(property => property.CanRead
                           && property.GetIndexParameters().Length == 0
                           && !string.Equals(property.Name, "EqualityContract", StringComparison.Ordinal));
}

static string FormatTypeName(Type type)
{
    if (type == typeof(string)) return "string";
    if (type == typeof(int)) return "int";
    if (type == typeof(long)) return "long";
    if (type == typeof(short)) return "short";
    if (type == typeof(byte)) return "byte";
    if (type == typeof(bool)) return "bool";
    if (type == typeof(decimal)) return "decimal";
    if (type == typeof(double)) return "double";
    if (type == typeof(float)) return "float";
    if (type == typeof(char)) return "char";
    if (type == typeof(object)) return "object";

    if (type.IsArray)
    {
        return $"{FormatTypeName(type.GetElementType()!)}[]";
    }

    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
    {
        return $"{FormatTypeName(type.GetGenericArguments()[0])}?";
    }

    if (type.IsGenericType)
    {
        var fullName = type.GetGenericTypeDefinition().FullName!;
        var tickIndex = fullName.IndexOf('`');
        if (tickIndex >= 0)
        {
            fullName = fullName[..tickIndex];
        }

        var genericArguments = string.Join(", ", type.GetGenericArguments().Select(FormatTypeName));
        return $"global::{fullName}<{genericArguments}>";
    }

    return $"global::{type.FullName}";
}

static string ToNullableLiteral(string? value)
{
    return value is null ? "null" : ToStringLiteral(value);
}

static string ToStringLiteral(string value)
{
    return "\"" + value
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("\"", "\\\"", StringComparison.Ordinal)
        .Replace("\r", "\\r", StringComparison.Ordinal)
        .Replace("\n", "\\n", StringComparison.Ordinal)
        .Replace("\t", "\\t", StringComparison.Ordinal)
        + "\"";
}

static string EscapeVerbatim(string value)
{
    return value.Replace("\\", "\\\\", StringComparison.Ordinal).Replace("\"", "\\\"", StringComparison.Ordinal);
}

internal sealed record UseCaseTypeMetadata(string TypeName, string RequestType);

internal sealed record CaseBlueprint(
    string EnumName,
    string TemplateName,
    string RequestCode,
    UseCaseCaseKind Kind,
    UseCaseHandlerExpectation HandlerExpectation,
    UseCaseValidatorExpectation ValidatorExpectation,
    bool UseTemplateScenario);
