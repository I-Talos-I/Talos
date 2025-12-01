namespace Talos.Tool.Models;

public class Dependency
{
    public string Name { get; set; } = string.Empty;

    public List<string> Version { get; set; } = new();

    public Dictionary<string, List<string>> Commands { get; set; } = new();

    public List<string> AvailableVersions { get; set; } = new();

    public List<CompatibleDependency> CompatibleWith { get; set; } = new();
}