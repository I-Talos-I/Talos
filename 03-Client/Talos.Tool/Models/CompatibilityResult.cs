namespace Talos.Tool.Models;

public class CompatibilityResult
{
    public string Package { get; set; } = string.Empty;
    public List<string> CompatibleVersions { get; set; } = new();
    public double CompatibilityScore { get; set; }
}