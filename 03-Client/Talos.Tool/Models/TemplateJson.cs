using System.Text.Json.Serialization;

namespace Talos.Tool.Models;

public class TemplateJson
{
    [JsonPropertyName("$schema")]
    public string? Schema { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("dependencies")]
    public List<Dependency> Dependencies { get; set; } = new();
}