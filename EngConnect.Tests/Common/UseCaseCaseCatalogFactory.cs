using System.Collections;

namespace EngConnect.Tests.Common;

public static class UseCaseCaseCatalogFactory
{
    public static UseCaseCaseCatalog CreateCatalog(UseCaseDefinition definition)
    {
        var hasHandler = !string.IsNullOrWhiteSpace(definition.HandlerTypeFullName);
        var hasValidator = !string.IsNullOrWhiteSpace(definition.ValidatorTypeFullName);
        var handlerSource = UseCaseSourceAnalyzer.ReadSource(definition.HandlerSourceRelativePath);
        var validatorSource = UseCaseSourceAnalyzer.ReadSource(definition.ValidatorSourceRelativePath);
        var defaultCatalog = new UseCaseCaseCatalog
        {
            ValidCases = BuildValidCases(definition, handlerSource, hasHandler, hasValidator),
            BoundaryCases = BuildBoundaryCases(definition, validatorSource, hasHandler, hasValidator),
            InvalidCases = BuildInvalidCases(definition, handlerSource, validatorSource, hasHandler, hasValidator),
            ExceptionCases = BuildExceptionCases(definition, hasHandler)
        };

        return UseCaseCustomCaseRegistry.Apply(definition, defaultCatalog);
    }

    private static IReadOnlyList<UseCaseCaseSet> BuildValidCases(
        UseCaseDefinition definition,
        string handlerSource,
        bool hasHandler,
        bool hasValidator)
    {
        var cases = new List<UseCaseCaseSet>
        {
            CreateCaseSet(
                name: "valid-default",
                kind: UseCaseCaseKind.Valid,
                scenario: UseCaseTestDataFactory.CreateValidScenario(definition.RequestTypeFullName),
                hasHandler ? UseCaseHandlerExpectation.Completes : UseCaseHandlerExpectation.Skip,
                hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip)
        };

        foreach (var optionalRule in UseCaseSourceAnalyzer.ExtractOptionalRequestRules(handlerSource))
        {
            cases.Add(CreateCaseSet(
                name: BuildOptionalCaseName(optionalRule),
                kind: UseCaseCaseKind.Valid,
                scenario: UseCaseTestDataFactory.CreateValidScenario(
                    definition.RequestTypeFullName,
                    request => ApplyOptionalRule(request, optionalRule)),
                hasHandler ? UseCaseHandlerExpectation.Completes : UseCaseHandlerExpectation.Skip,
                hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip));
        }

        return Deduplicate(cases);
    }

    private static IReadOnlyList<UseCaseCaseSet> BuildBoundaryCases(
        UseCaseDefinition definition,
        string validatorSource,
        bool hasHandler,
        bool hasValidator)
    {
        var rules = UseCaseSourceAnalyzer.ExtractValidBoundaryRules(validatorSource);
        var cases = new List<UseCaseCaseSet>();

        if (rules.Count == 0)
        {
            cases.Add(CreateCaseSet(
                name: "boundary-default",
                kind: UseCaseCaseKind.Boundary,
                scenario: UseCaseTestDataFactory.CreateValidScenario(definition.RequestTypeFullName),
                hasHandler ? UseCaseHandlerExpectation.Completes : UseCaseHandlerExpectation.Skip,
                hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip));

            return cases;
        }

        foreach (var rule in rules)
        {
            cases.Add(CreateCaseSet(
                name: $"boundary-{rule.PropertyPath.Replace('.', '-')}-{rule.Value}",
                kind: UseCaseCaseKind.Boundary,
                scenario: UseCaseTestDataFactory.CreateBoundaryScenario(
                    definition.RequestTypeFullName,
                    request => ApplyBoundaryRule(request, rule)),
                hasHandler ? UseCaseHandlerExpectation.Completes : UseCaseHandlerExpectation.Skip,
                hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip));
        }

        return Deduplicate(cases);
    }

    private static IReadOnlyList<UseCaseCaseSet> BuildInvalidCases(
        UseCaseDefinition definition,
        string handlerSource,
        string validatorSource,
        bool hasHandler,
        bool hasValidator)
    {
        var cases = new List<UseCaseCaseSet>();
        var requestGuardRules = UseCaseSourceAnalyzer.ExtractRequestGuardRules(handlerSource);

        if (requestGuardRules.Count > 0)
        {
            foreach (var requestGuardRule in requestGuardRules)
            {
                cases.Add(CreateCaseSet(
                    name: $"invalid-request-guard-{requestGuardRule.PropertyPath.Replace('.', '-')}",
                    kind: UseCaseCaseKind.Invalid,
                    scenario: UseCaseTestDataFactory.CreateValidScenario(
                        definition.RequestTypeFullName,
                        request => ApplyRequestGuardRule(request, requestGuardRule)),
                    hasHandler ? UseCaseHandlerExpectation.Failure : UseCaseHandlerExpectation.Skip,
                    hasValidator ? UseCaseValidatorExpectation.Fail : UseCaseValidatorExpectation.Skip));
            }
        }
        else if (hasValidator)
        {
            cases.Add(CreateCaseSet(
                name: "invalid-request-shape",
                kind: UseCaseCaseKind.Invalid,
                scenario: UseCaseTestDataFactory.CreateInvalidScenario(definition.RequestTypeFullName),
                UseCaseHandlerExpectation.Skip,
                UseCaseValidatorExpectation.Fail));
        }

        if (hasValidator)
        {
            foreach (var rule in UseCaseSourceAnalyzer.ExtractInvalidBoundaryRules(validatorSource))
            {
                cases.Add(CreateCaseSet(
                    name: $"invalid-boundary-{rule.PropertyPath.Replace('.', '-')}-{rule.Value}",
                    kind: UseCaseCaseKind.Invalid,
                    scenario: UseCaseTestDataFactory.CreateBoundaryScenario(
                        definition.RequestTypeFullName,
                        request => ApplyBoundaryRule(request, rule)),
                    UseCaseHandlerExpectation.Skip,
                    UseCaseValidatorExpectation.Fail));
            }
        }

        var negativeExistenceRules = UseCaseSourceAnalyzer.ExtractNegativeExistenceRules(handlerSource);
        if (hasHandler && negativeExistenceRules.Count > 0)
        {
            foreach (var existenceRule in negativeExistenceRules)
            {
                cases.Add(CreateCaseSet(
                    name: $"invalid-{existenceRule.Name}-missing",
                    kind: UseCaseCaseKind.Invalid,
                    scenario: UseCaseTestDataFactory.CreateValidScenario(
                        definition.RequestTypeFullName,
                        request => ApplyExistenceRule(request, existenceRule, shouldMatch: false)),
                    UseCaseHandlerExpectation.Failure,
                    hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip));
            }
        }
        else if (hasHandler && (UseCaseSourceAnalyzer.HasNegativeExistenceBranch(handlerSource) || UseCaseSourceAnalyzer.HasLookupFailureBranch(handlerSource)))
        {
            cases.Add(CreateCaseSet(
                name: "invalid-not-found-or-null",
                kind: UseCaseCaseKind.Invalid,
                scenario: UseCaseTestDataFactory.CreateValidScenario(
                    definition.RequestTypeFullName,
                    arrangeMocks: mocks => mocks.UseSeededUnitOfWork(seedData: false)),
                UseCaseHandlerExpectation.Failure,
                hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip));
        }

        var positiveExistenceRules = UseCaseSourceAnalyzer.ExtractPositiveExistenceRules(handlerSource);
        if (hasHandler && positiveExistenceRules.Count > 0)
        {
            foreach (var existenceRule in positiveExistenceRules)
            {
                cases.Add(CreateCaseSet(
                    name: $"invalid-{existenceRule.Name}-existing",
                    kind: UseCaseCaseKind.Invalid,
                    scenario: UseCaseTestDataFactory.CreateValidScenario(
                        definition.RequestTypeFullName,
                        request => ApplyExistenceRule(request, existenceRule, shouldMatch: true)),
                    UseCaseHandlerExpectation.Failure,
                    hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip));
            }
        }
        else if (hasHandler && UseCaseSourceAnalyzer.HasPositiveExistenceBranch(handlerSource))
        {
            cases.Add(CreateCaseSet(
                name: "invalid-duplicate-or-existing",
                kind: UseCaseCaseKind.Invalid,
                scenario: UseCaseTestDataFactory.CreateDuplicateScenario(definition.RequestTypeFullName),
                UseCaseHandlerExpectation.Failure,
                hasValidator ? UseCaseValidatorExpectation.Pass : UseCaseValidatorExpectation.Skip));
        }

        return Deduplicate(cases);
    }

    private static IReadOnlyList<UseCaseCaseSet> BuildExceptionCases(UseCaseDefinition definition, bool hasHandler)
    {
        if (!hasHandler)
        {
            return [];
        }

        return
        [
            CreateCaseSet(
                name: "exception-path",
                kind: UseCaseCaseKind.Exception,
                scenario: UseCaseTestDataFactory.CreateValidScenario(definition.RequestTypeFullName),
                UseCaseHandlerExpectation.Exception,
                UseCaseValidatorExpectation.Skip)
        ];
    }

    private static IReadOnlyList<UseCaseCaseSet> Deduplicate(IEnumerable<UseCaseCaseSet> cases)
    {
        return cases
            .GroupBy(caseSet => $"{caseSet.Kind}|{caseSet.Name}", StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();
    }

    private static UseCaseCaseSet CreateCaseSet(
        string name,
        UseCaseCaseKind kind,
        UseCaseScenario scenario,
        UseCaseHandlerExpectation handlerExpectation,
        UseCaseValidatorExpectation validatorExpectation)
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
                Scenario = scenario
            }
        };
    }

    private static void ApplyBoundaryRule(object request, UseCaseBoundaryRule rule)
    {
        switch (rule.Kind)
        {
            case UseCaseBoundaryKind.StringLength:
                ObjectGraphAccessor.SetValue(request, rule.PropertyPath, BuildBoundaryString(rule.PropertyPath, rule.Value));
                break;
            case UseCaseBoundaryKind.IntegerValue:
                var propertyType = ResolvePropertyType(request, rule.PropertyPath);
                var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                ObjectGraphAccessor.SetValue(request, rule.PropertyPath, Convert.ChangeType(rule.Value, targetType));
                break;
        }
    }

    private static void ApplyRequestGuardRule(object request, UseCaseRequestGuardRule rule)
    {
        if (!TryResolvePropertyType(request, rule.PropertyPath, out var propertyType))
        {
            return;
        }

        var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (targetType == typeof(string))
        {
            ObjectGraphAccessor.SetValue(request, rule.PropertyPath, string.Empty);
            return;
        }

        if (targetType == typeof(Guid))
        {
            ObjectGraphAccessor.SetValue(request, rule.PropertyPath, Guid.Empty);
            return;
        }

        if (propertyType.IsArray)
        {
            ObjectGraphAccessor.SetValue(request, rule.PropertyPath, Array.CreateInstance(propertyType.GetElementType()!, 0));
            return;
        }

        if (CanBeNull(propertyType))
        {
            ObjectGraphAccessor.SetValue(request, rule.PropertyPath, null);
            return;
        }

        ObjectGraphAccessor.SetValue(request, rule.PropertyPath, Activator.CreateInstance(targetType));
    }

    private static void ApplyExistenceRule(object request, UseCaseExistenceRule rule, bool shouldMatch)
    {
        foreach (var propertyPath in rule.PropertyPaths)
        {
            if (!TryResolvePropertyType(request, propertyPath, out var propertyType))
            {
                continue;
            }

            var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (targetType == typeof(string))
            {
                ObjectGraphAccessor.SetValue(request, propertyPath, shouldMatch
                    ? BuildSeedString(propertyPath)
                    : BuildMissingString(propertyPath));
                continue;
            }

            if (targetType == typeof(Guid))
            {
                ObjectGraphAccessor.SetValue(request, propertyPath, shouldMatch
                    ? UseCaseSeedDefaults.KnownGuid
                    : Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"));
                continue;
            }

            if (targetType == typeof(int))
            {
                ObjectGraphAccessor.SetValue(request, propertyPath, shouldMatch ? 1 : -1);
                continue;
            }

            if (targetType == typeof(long))
            {
                ObjectGraphAccessor.SetValue(request, propertyPath, shouldMatch ? 1L : -1L);
            }
        }
    }

    private static void ApplyOptionalRule(object request, UseCaseOptionalRule rule)
    {
        var propertyType = ResolvePropertyType(request, rule.PropertyPath);

        switch (rule.Kind)
        {
            case UseCaseOptionalRuleKind.Null:
                ObjectGraphAccessor.SetValue(request, rule.PropertyPath, CanBeNull(propertyType) ? null : Activator.CreateInstance(propertyType));
                break;
            case UseCaseOptionalRuleKind.EmptyString:
                if (propertyType == typeof(string))
                {
                    ObjectGraphAccessor.SetValue(request, rule.PropertyPath, string.Empty);
                }
                else
                {
                    ObjectGraphAccessor.SetValue(request, rule.PropertyPath, CanBeNull(propertyType) ? null : Activator.CreateInstance(propertyType));
                }
                break;
            case UseCaseOptionalRuleKind.EmptyArray:
                ObjectGraphAccessor.SetValue(request, rule.PropertyPath, CreateEmptyCollection(propertyType));
                break;
        }
    }

    private static object? CreateEmptyCollection(Type propertyType)
    {
        if (propertyType.IsArray)
        {
            return Array.CreateInstance(propertyType.GetElementType()!, 0);
        }

        if (propertyType.IsGenericType)
        {
            var definition = propertyType.GetGenericTypeDefinition();
            var elementType = propertyType.GetGenericArguments()[0];

            if (definition == typeof(List<>)
                || definition == typeof(IList<>)
                || definition == typeof(IEnumerable<>)
                || definition == typeof(ICollection<>)
                || definition == typeof(IReadOnlyList<>))
            {
                var listType = typeof(List<>).MakeGenericType(elementType);
                return Activator.CreateInstance(listType);
            }
        }

        if (typeof(IEnumerable).IsAssignableFrom(propertyType))
        {
            return Activator.CreateInstance(propertyType);
        }

        return null;
    }

    private static bool CanBeNull(Type propertyType)
    {
        return !propertyType.IsValueType || Nullable.GetUnderlyingType(propertyType) != null;
    }

    private static Type ResolvePropertyType(object root, string propertyPath)
    {
        var currentType = root.GetType();
        foreach (var segment in propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            var property = currentType.GetProperty(segment)
                           ?? throw new InvalidOperationException($"Property {segment} was not found on {currentType.FullName}.");
            currentType = property.PropertyType;
        }

        return currentType;
    }

    private static bool TryResolvePropertyType(object root, string propertyPath, out Type propertyType)
    {
        try
        {
            propertyType = ResolvePropertyType(root, propertyPath);
            return true;
        }
        catch
        {
            propertyType = typeof(object);
            return false;
        }
    }

    private static string BuildOptionalCaseName(UseCaseOptionalRule rule)
    {
        var path = rule.PropertyPath.Replace('.', '-');
        return rule.Kind switch
        {
            UseCaseOptionalRuleKind.Null => $"valid-without-{path}",
            UseCaseOptionalRuleKind.EmptyString => $"valid-empty-{path}",
            UseCaseOptionalRuleKind.EmptyArray => $"valid-empty-{path}",
            _ => $"valid-variant-{path}"
        };
    }

    private static string BuildBoundaryString(string propertyPath, int length)
    {
        var safeLength = Math.Max(length, 0);
        if (safeLength == 0)
        {
            return string.Empty;
        }

        if (propertyPath.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            const string suffix = "@example.com";
            if (safeLength <= suffix.Length)
            {
                return safeLength == 0 ? string.Empty : UseCaseSeedDefaults.KnownEmail;
            }

            return new string('a', safeLength - suffix.Length) + suffix;
        }

        if (propertyPath.Contains("Url", StringComparison.OrdinalIgnoreCase)
            || propertyPath.Contains("Avatar", StringComparison.OrdinalIgnoreCase))
        {
            const string prefix = "https://example.com/";
            if (safeLength <= prefix.Length)
            {
                return safeLength == 0 ? string.Empty : UseCaseSeedDefaults.KnownUrl;
            }

            return prefix + new string('u', safeLength - prefix.Length);
        }

        if (propertyPath.Contains("Phone", StringComparison.OrdinalIgnoreCase))
        {
            const string digits = "012345678901234567890123456789";
            return digits[..Math.Min(safeLength, digits.Length)];
        }

        return new string('b', safeLength);
    }

    private static string BuildSeedString(string propertyPath)
    {
        if (propertyPath.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownEmail;
        }

        if (propertyPath.Contains("Phone", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownPhone;
        }

        if (propertyPath.Contains("Code", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownCode;
        }

        if (propertyPath.Contains("Status", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownStatus;
        }

        if (propertyPath.Contains("Type", StringComparison.OrdinalIgnoreCase))
        {
            return UseCaseSeedDefaults.KnownString;
        }

        return UseCaseSeedDefaults.KnownString;
    }

    private static string BuildMissingString(string propertyPath)
    {
        if (propertyPath.Contains("Email", StringComparison.OrdinalIgnoreCase))
        {
            return "missing@example.com";
        }

        if (propertyPath.Contains("Phone", StringComparison.OrdinalIgnoreCase))
        {
            return "0999999999";
        }

        if (propertyPath.Contains("Code", StringComparison.OrdinalIgnoreCase))
        {
            return "MISSING-CODE";
        }

        if (propertyPath.Contains("Type", StringComparison.OrdinalIgnoreCase))
        {
            return "missing-type";
        }

        return "missing-value";
    }
}
