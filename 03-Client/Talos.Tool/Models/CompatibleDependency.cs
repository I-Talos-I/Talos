namespace Talos.Tool.Models;

public class CompatibleDependency
{
    public string Name { get; set; } = string.Empty;
    public List<string> Versions { get; set; } = new();
}