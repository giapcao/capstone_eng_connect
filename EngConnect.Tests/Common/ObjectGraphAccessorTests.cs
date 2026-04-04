using Xunit;

namespace EngConnect.Tests.Common;

public class ObjectGraphAccessorTests
{
    [Fact]
    public void SetValue_creates_intermediate_object_and_sets_nested_property()
    {
        var root = new RootNode();

        ObjectGraphAccessor.SetValue(root, "Child.Name", "patched");

        Assert.NotNull(root.Child);
        Assert.Equal("patched", root.Child!.Name);
    }

    [Fact]
    public void SetValue_throws_for_unknown_path()
    {
        var root = new RootNode();

        var action = () => ObjectGraphAccessor.SetValue(root, "Unknown.Name", "patched");

        var exception = Assert.Throws<InvalidOperationException>(action);
        Assert.Contains("Property Unknown was not found", exception.Message);
    }

    private sealed class RootNode
    {
        public ChildNode? Child { get; set; }
    }

    private sealed class ChildNode
    {
        public string? Name { get; set; }
    }
}
