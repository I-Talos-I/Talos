namespace Talos.Tool.Models;

public class Dependency
{
    public string Name { get; set; } = string.Empty;
    public string? Version { get; set; }
    public Dictionary<string, string[]> Commands { get; set; } = new();
    public List<string> AvailableVersions { get; set; } = new();
    public List<CompatibleDependency> CompatibleWith { get; set; } = new();
}