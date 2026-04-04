namespace EngConnect.Tests.Common;

internal static class TestPathHelper
{
    private static readonly string RepoRoot = ResolveRepoRoot();

    public static string GetRepoPath(string relativePath)
    {
        var normalized = relativePath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        return Path.Combine(RepoRoot, normalized);
    }

    private static string ResolveRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            if (Directory.Exists(Path.Combine(current.FullName, "EngConnect.Application"))
                && Directory.Exists(Path.Combine(current.FullName, "EngConnect.Tests")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException($"Cannot locate repository root from base directory '{AppContext.BaseDirectory}'.");
    }
}
