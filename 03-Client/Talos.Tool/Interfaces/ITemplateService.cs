using Talos.Tool.Models;

namespace Talos.Tool.Interfaces;

public interface ITemplateService
{
    Task<TemplateJson?> GetTemplateAsync(string templateSlug);
    Task<List<CompatibilityResult>?> GetCompatibleVersionsAsync(string package, string version);
}