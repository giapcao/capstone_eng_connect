using System.Text.RegularExpressions;

namespace EngConnect.Tests.Common;

internal static class UseCaseSourceAnalyzer
{
    private static readonly Regex AnyAsyncAssignmentRegex = new(
        @"var\s+(?<name>\w+)\s*=\s*await[\s\S]*?AnyAsync\(",
        RegexOptions.Compiled);

    private static readonly Regex RequestPropertyRegex = new(
        @"(?:command|query)\.(?<path>[\w\.]+)(?!\s*\()",
        RegexOptions.Compiled);

    private static readonly Regex RequestGuardRegex = new(
        @"if\s*\(\s*(?:(?:ValidationUtil\.IsNullOrEmpty|string\.IsNullOrWhiteSpace)\(\s*(?:command|query)\.[\w\.]+\s*\)"
        + @"|(?:command|query)\.[\w\.]+\s*(?:==\s*null|is\s+null))\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex RequestGuardExtractionRegex = new(
        @"if\s*\(\s*(?:(?:ValidationUtil\.IsNullOrEmpty|string\.IsNullOrWhiteSpace)\(\s*(?:command|query)\.(?<path1>[\w\.]+)\s*\)"
        + @"|(?:command|query)\.(?<path2>[\w\.]+)\s*(?:==\s*null|is\s+null))\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex LookupMethodRegex = new(
        @"(FindByIdAsync|FindSingleAsync|FindFirstAsync|FirstOrDefaultAsync|SingleOrDefaultAsync)\(",
        RegexOptions.Compiled);

    private static readonly Regex AnyAsyncExistenceRuleRegex = new(
        @"var\s+(?<name>\w+)\s*=\s*await[\s\S]*?AnyAsync\((?<predicate>[\s\S]*?)\)\s*;\s*if\s*\(\s*(?<neg>!?)\s*\k<name>\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex LookupNullExistenceRuleRegex = new(
        @"var\s+(?<name>\w+)\s*=\s*await[\s\S]*?(?<method>FindByIdAsync|FindSingleAsync|FindFirstAsync|FirstOrDefaultAsync|SingleOrDefaultAsync)\((?<predicate>[\s\S]*?)\)\s*;\s*if\s*\(\s*(?:ValidationUtil\.IsNullOrEmpty\(\s*\k<name>\s*\)|\k<name>\s*(?:==\s*null|is\s+null))\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex NullFailureBranchRegex = new(
        @"if\s*\(\s*[^)]*(?:==\s*null|is\s+null|ValidationUtil\.IsNullOrEmpty\([^)]*\))[^)]*\)\s*(?:\{[\s\S]*?return\s+Result\.Failure|return\s+Result\.Failure)",
        RegexOptions.Compiled);

    private static readonly Regex MaxLengthRegex = new(
        @"RuleFor\(\s*x\s*=>\s*x\.(?<path>[\w\.]+)\s*\)(?<body>(?:(?!RuleFor\().)*?)MaximumLength\((?<value>\d+)\)",
        RegexOptions.Compiled);

    private static readonly Regex MinLengthRegex = new(
        @"RuleFor\(\s*x\s*=>\s*x\.(?<path>[\w\.]+)\s*\)(?<body>(?:(?!RuleFor\().)*?)MinimumLength\((?<value>\d+)\)",
        RegexOptions.Compiled);

    private static readonly Regex InclusiveBetweenRegex = new(
        @"RuleFor\(\s*x\s*=>\s*x\.(?<path>[\w\.]+)\s*\)(?<body>(?:(?!RuleFor\().)*?)InclusiveBetween\((?<min>\-?\d+)\s*,\s*(?<max>\-?\d+)\)",
        RegexOptions.Compiled);

    private static readonly Regex OptionalNotNullRegex = new(
        @"if\s*\(\s*(?:command|query)\.(?<path>[\w\.]+)\s*!=\s*null\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex OptionalHasValueRegex = new(
        @"if\s*\(\s*(?:command|query)\.(?<path>[\w\.]+)\.HasValue\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex OptionalNotWhiteSpaceRegex = new(
        @"if\s*\(\s*!\s*string\.IsNullOrWhiteSpace\(\s*(?:command|query)\.(?<path>[\w\.]+)\s*\)\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex OptionalValidationUtilNotNullOrEmptyRegex = new(
        @"if\s*\(\s*ValidationUtil\.IsNotNullOrEmpty\(\s*(?:command|query)\.(?<path>[\w\.]+)\s*\)\s*\)",
        RegexOptions.Compiled);

    private static readonly Regex OptionalNonEmptyArrayRegex = new(
        @"if\s*\(\s*(?:command|query)\.(?<path>[\w\.]+)\s*is\s*\{\s*Length\s*:\s*>\s*0\s*\}\s*\)",
        RegexOptions.Compiled);

    public static string ReadSource(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return string.Empty;
        }

        var fullPath = TestPathHelper.GetRepoPath(relativePath);
        return File.Exists(fullPath) ? StripComments(File.ReadAllText(fullPath)) : string.Empty;
    }

    public static bool HasPositiveExistenceBranch(string source)
    {
        return AnyAsyncAssignmentRegex
            .Matches(source)
            .Cast<Match>()
            .Select(match => match.Groups["name"].Value)
            .Any(variableName => HasIfGuard(source, variableName, positive: true));
    }

    public static bool HasNegativeExistenceBranch(string source)
    {
        return AnyAsyncAssignmentRegex
            .Matches(source)
            .Cast<Match>()
            .Select(match => match.Groups["name"].Value)
            .Any(variableName => HasIfGuard(source, variableName, positive: false));
    }

    public static bool HasRequestGuardBranch(string source)
    {
        return RequestGuardRegex.IsMatch(source);
    }

    public static bool HasLookupFailureBranch(string source)
    {
        return LookupMethodRegex.IsMatch(source) && NullFailureBranchRegex.IsMatch(source);
    }

    public static IReadOnlyList<UseCaseRequestGuardRule> ExtractRequestGuardRules(string source)
    {
        var rules = new List<UseCaseRequestGuardRule>();

        foreach (Match match in RequestGuardExtractionRegex.Matches(source))
        {
            var path = match.Groups["path1"].Success
                ? match.Groups["path1"].Value
                : match.Groups["path2"].Value;

            if (!string.IsNullOrWhiteSpace(path))
            {
                rules.Add(new UseCaseRequestGuardRule(path));
            }
        }

        return rules
            .GroupBy(rule => rule.PropertyPath, StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();
    }

    public static IReadOnlyList<UseCaseExistenceRule> ExtractPositiveExistenceRules(string source)
    {
        return ExtractExistenceRules(source, positiveBranch: true);
    }

    public static IReadOnlyList<UseCaseExistenceRule> ExtractNegativeExistenceRules(string source)
    {
        return ExtractExistenceRules(source, positiveBranch: false);
    }

    public static IReadOnlyList<UseCaseBoundaryRule> ExtractValidBoundaryRules(string source)
    {
        var rules = new List<UseCaseBoundaryRule>();

        foreach (Match match in MaxLengthRegex.Matches(source))
        {
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.StringLength, int.Parse(match.Groups["value"].Value)));
        }

        foreach (Match match in MinLengthRegex.Matches(source))
        {
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.StringLength, int.Parse(match.Groups["value"].Value)));
        }

        foreach (Match match in InclusiveBetweenRegex.Matches(source))
        {
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.IntegerValue, int.Parse(match.Groups["min"].Value)));
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.IntegerValue, int.Parse(match.Groups["max"].Value)));
        }

        return DistinctRules(rules);
    }

    public static IReadOnlyList<UseCaseBoundaryRule> ExtractInvalidBoundaryRules(string source)
    {
        var rules = new List<UseCaseBoundaryRule>();

        foreach (Match match in MaxLengthRegex.Matches(source))
        {
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.StringLength, int.Parse(match.Groups["value"].Value) + 1));
        }

        foreach (Match match in MinLengthRegex.Matches(source))
        {
            var minValue = int.Parse(match.Groups["value"].Value);
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.StringLength, Math.Max(minValue - 1, 0)));
        }

        foreach (Match match in InclusiveBetweenRegex.Matches(source))
        {
            var minValue = int.Parse(match.Groups["min"].Value);
            var maxValue = int.Parse(match.Groups["max"].Value);
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.IntegerValue, minValue - 1));
            rules.Add(new UseCaseBoundaryRule(match.Groups["path"].Value, UseCaseBoundaryKind.IntegerValue, maxValue + 1));
        }

        return DistinctRules(rules);
    }

    public static IReadOnlyList<UseCaseOptionalRule> ExtractOptionalRequestRules(string source)
    {
        var rules = new List<UseCaseOptionalRule>();

        AddOptionalRules(rules, OptionalNotNullRegex, UseCaseOptionalRuleKind.Null);
        AddOptionalRules(rules, OptionalHasValueRegex, UseCaseOptionalRuleKind.Null);
        AddOptionalRules(rules, OptionalNotWhiteSpaceRegex, UseCaseOptionalRuleKind.EmptyString);
        AddOptionalRules(rules, OptionalValidationUtilNotNullOrEmptyRegex, UseCaseOptionalRuleKind.EmptyString);
        AddOptionalRules(rules, OptionalNonEmptyArrayRegex, UseCaseOptionalRuleKind.EmptyArray);

        return rules
            .GroupBy(rule => $"{rule.PropertyPath}|{rule.Kind}", StringComparer.Ordinal)
            .Select(group => group.First())
            .Take(6)
            .ToList();

        void AddOptionalRules(ICollection<UseCaseOptionalRule> target, Regex regex, UseCaseOptionalRuleKind kind)
        {
            foreach (Match match in regex.Matches(source))
            {
                target.Add(new UseCaseOptionalRule(match.Groups["path"].Value, kind));
            }
        }
    }

    private static IReadOnlyList<UseCaseBoundaryRule> DistinctRules(IEnumerable<UseCaseBoundaryRule> rules)
    {
        return rules
            .GroupBy(rule => $"{rule.PropertyPath}|{rule.Kind}|{rule.Value}", StringComparer.Ordinal)
            .Select(group => group.First())
            .Take(8)
            .ToList();
    }

    private static bool HasIfGuard(string source, string variableName, bool positive)
    {
        var escapedVariable = Regex.Escape(variableName);
        var pattern = positive
            ? $@"if\s*\(\s*{escapedVariable}\s*\)|if\s*\(\s*{escapedVariable}\s*==\s*true\s*\)|ValidationUtil\.IsNotNullOrEmpty\(\s*{escapedVariable}\s*\)"
            : $@"if\s*\(\s*!\s*{escapedVariable}\s*\)|if\s*\(\s*{escapedVariable}\s*==\s*false\s*\)|ValidationUtil\.IsNullOrEmpty\(\s*{escapedVariable}\s*\)";

        return Regex.IsMatch(source, pattern);
    }

    private static string StripComments(string source)
    {
        var withoutBlockComments = Regex.Replace(source, @"/\*[\s\S]*?\*/", string.Empty);
        return Regex.Replace(withoutBlockComments, @"//.*", string.Empty);
    }

    private static IReadOnlyList<UseCaseExistenceRule> ExtractExistenceRules(string source, bool positiveBranch)
    {
        var rules = new List<UseCaseExistenceRule>();

        foreach (Match match in AnyAsyncExistenceRuleRegex.Matches(source))
        {
            var isPositive = string.IsNullOrWhiteSpace(match.Groups["neg"].Value);
            if (isPositive != positiveBranch)
            {
                continue;
            }

            var propertyPaths = ExtractRequestPropertyPaths(match.Groups["predicate"].Value);
            if (propertyPaths.Count == 0)
            {
                continue;
            }

            rules.Add(new UseCaseExistenceRule(
                match.Groups["name"].Value,
                UseCaseExistenceSourceKind.AnyAsync,
                isPositive ? UseCaseExistenceBranchKind.Positive : UseCaseExistenceBranchKind.Negative,
                propertyPaths));
        }

        if (!positiveBranch)
        {
            foreach (Match match in LookupNullExistenceRuleRegex.Matches(source))
            {
                var propertyPaths = ExtractRequestPropertyPaths(match.Groups["predicate"].Value);
                if (propertyPaths.Count == 0)
                {
                    continue;
                }

                rules.Add(new UseCaseExistenceRule(
                    match.Groups["name"].Value,
                    UseCaseExistenceSourceKind.Lookup,
                    UseCaseExistenceBranchKind.Negative,
                    propertyPaths));
            }
        }

        return rules
            .GroupBy(rule => $"{rule.SourceKind}|{rule.BranchKind}|{string.Join(",", rule.PropertyPaths)}", StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();
    }

    private static IReadOnlyList<string> ExtractRequestPropertyPaths(string sourceFragment)
    {
        return RequestPropertyRegex.Matches(sourceFragment)
            .Cast<Match>()
            .Select(match => match.Groups["path"].Value)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Distinct(StringComparer.Ordinal)
            .Take(4)
            .ToList();
    }
}

internal sealed record UseCaseBoundaryRule(string PropertyPath, UseCaseBoundaryKind Kind, int Value);

internal enum UseCaseBoundaryKind
{
    StringLength,
    IntegerValue
}

internal sealed record UseCaseOptionalRule(string PropertyPath, UseCaseOptionalRuleKind Kind);

internal enum UseCaseOptionalRuleKind
{
    Null,
    EmptyString,
    EmptyArray
}

internal sealed record UseCaseRequestGuardRule(string PropertyPath);

internal sealed record UseCaseExistenceRule(
    string Name,
    UseCaseExistenceSourceKind SourceKind,
    UseCaseExistenceBranchKind BranchKind,
    IReadOnlyList<string> PropertyPaths);

internal enum UseCaseExistenceSourceKind
{
    AnyAsync,
    Lookup
}

internal enum UseCaseExistenceBranchKind
{
    Positive,
    Negative
}
