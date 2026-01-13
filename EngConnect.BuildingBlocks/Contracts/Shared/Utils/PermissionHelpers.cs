using System.Reflection;

namespace EngConnect.BuildingBlock.Contracts.Shared.Utils;

public static class PermissionHelpers
{
    #region Constants
    
    //TODO: Define your permissions here
    
    #endregion

    public static IEnumerable<string> GetAllPermissions()
    {
        // Register all features as policies
        var featureType = typeof(PermissionHelpers);
        var features = featureType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi is { IsLiteral: true, IsInitOnly: false } && fi.FieldType == typeof(string))
            .Select(x => x.GetRawConstantValue() as string ?? "")
            // Remove empty features
            .Where(x => !string.IsNullOrEmpty(x));
        return features;
    }

    public static IEnumerable<PermissionGroup> GetAllPermissionsGrouped()
    {
        var features = GetAllPermissions();
        return features.GroupBy(x => x.Split('.')[0])
            .Select(x => new PermissionGroup
            {
                Code = x.Key,
                Permissions = x.ToList()
            });
    }

    public static IEnumerable<PermissionGroup> GetAllPermissionsGrouped(IEnumerable<string> features)
    {
        return features.GroupBy(x => x.Split('.')[0])
            .Select(x => new PermissionGroup
            {
                Code = x.Key,
                Permissions = x.ToList()
            });
    }
}

public class PermissionGroup
{
    public string Code { get; set; } = null!;
    public IEnumerable<string> Permissions { get; set; } = [];
}