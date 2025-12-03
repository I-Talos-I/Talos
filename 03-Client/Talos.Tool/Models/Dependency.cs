using System.Text.Json.Serialization;

namespace Talos.Tool.Models;

public class Dependency
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("version")]
    public List<string> Version { get; set; } = new();
    
    [JsonPropertyName("commands")]
    public Dictionary<string, List<string>> Commands { get; set; } = new();

    public List<string> AvailableVersions { get; set; } = new();

    public List<CompatibleDependency> CompatibleWith { get; set; } = new();
}