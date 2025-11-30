namespace Talos.Tool.Models;

public class TemplateJson
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public List<Dependency> Dependencies { get; set; } = new();
}